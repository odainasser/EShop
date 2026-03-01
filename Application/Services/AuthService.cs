using Application.Common.Interfaces;
using Application.Features.Auth;
using Application.Features.UserLogs;
using Domain.Entities;
using Domain.Constants;
using Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly IAppConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;
    private readonly IUserLogService _userLogService;

    public AuthService(
        IIdentityService identityService,
        ITokenService tokenService,
        IEmailService emailService,
        IAppConfiguration configuration,
        ILogger<AuthService> logger,
        IUserLogService userLogService)
    {
        _identityService = identityService;
        _tokenService = tokenService;
        _emailService = emailService;
        _configuration = configuration;
        _logger = logger;
        _userLogService = userLogService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _identityService.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "User with this email already exists"
            };
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            IsActive = true,
            EmailConfirmed = false,
            CreatedAt = DateTime.UtcNow
        };

        var (success, errors, createdUser) = await _identityService.CreateUserAsync(user, request.Password);

        if (!success)
        {
            return new AuthResponse
            {
                Success = false,
                Message = string.Join(", ", errors)
            };
        }

        // Assign Client role by default for public registration
        await _identityService.AddToRoleAsync(createdUser!.Id, Roles.Client);

        var roles = await _identityService.GetUserRolesAsync(createdUser!.Id);
        var permissions = await _identityService.GetUserPermissionsAsync(createdUser.Id);
        var token = _tokenService.GenerateJwtToken(createdUser.Id, createdUser.Email, roles, permissions);

        var createdUserDisplay = GetDisplayName(createdUser);
        await _userLogService.LogAsync(new CreateUserLogRequest
        {
            UserId = createdUser.Id,
            UserName = createdUserDisplay,
            Action = AuditAction.Created,
            EntityName = "User",
            EntityId = createdUser.Id.ToString()
        });

        return new AuthResponse
        {
            Success = true,
            Message = "Registration successful",
            Token = token,
            User = new UserDto
            {
                Id = createdUser!.Id,
                Email = createdUser.Email,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                DisplayName = createdUserDisplay,
                IsActive = createdUser.IsActive,
                Roles = roles.ToList(),
                Permissions = permissions.ToList()
            }
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        _logger.LogInformation("Login attempt for email: {Email}", request.Email);

        var (success, message, user) = await _identityService.ValidateCredentialsAsync(request.Email, request.Password);

        if (!success || user == null)
        {
            _logger.LogWarning("Login failed for email: {Email}. Reason: {Message}", request.Email, message);
            return new AuthResponse
            {
                Success = false,
                Message = message
            };
        }

        _logger.LogInformation("Login successful for email: {Email}, UserId: {UserId}", request.Email, user.Id);

        var roles = await _identityService.GetUserRolesAsync(user.Id);
        var permissions = await _identityService.GetUserPermissionsAsync(user.Id);
        var token = _tokenService.GenerateJwtToken(user.Id, user.Email, roles, permissions);

        var userDisplay = GetDisplayName(user);
        await _userLogService.LogAsync(new CreateUserLogRequest
        {
            UserId = user.Id,
            UserName = user.Email ?? userDisplay,
            Action = AuditAction.LoggedIn,
            EntityName = "User",
            EntityId = user.Id.ToString()
        });

        return new AuthResponse
        {
            Success = true,
            Message = "Login successful",
            Token = token,
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DisplayName = userDisplay,
                IsActive = user.IsActive,
                Roles = roles.ToList(),
                Permissions = permissions.ToList()
            }
        };
    }

    #region Email Confirmation

    public async Task<AuthResponse> SendEmailConfirmationAsync(string email)
    {
        var user = await _identityService.FindByEmailAsync(email);
        if (user == null)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "User not found"
            };
        }

        var token = await _identityService.GenerateEmailConfirmationTokenAsync(user.Id);
        var appUrl = _configuration.GetAppUrl();
        var confirmationLink = $"{appUrl}/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";
        
        await _emailService.SendEmailConfirmationAsync(user.Email, confirmationLink);

        return new AuthResponse
        {
            Success = true,
            Message = "Confirmation email sent successfully"
        };
    }

    public async Task<AuthResponse> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        if (!Guid.TryParse(request.UserId, out var userId))
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Invalid user ID"
            };
        }

        var (success, errors) = await _identityService.ConfirmEmailAsync(userId, request.Token);

        if (success)
        {
            var user = await _identityService.FindByIdAsync(userId);
            if (user != null)
            {
                var userDisplay = GetDisplayName(user);
                await _userLogService.LogAsync(new CreateUserLogRequest
                {
                    UserId = user.Id,
                    UserName = userDisplay,
                    Action = AuditAction.EmailVerified,
                    EntityName = "User",
                    EntityId = user.Id.ToString()
                });
            }
        }

        return new AuthResponse
        {
            Success = success,
            Message = success ? "Email confirmed successfully" : string.Join(", ", errors)
        };
    }

    #endregion

    #region Password Management

    public async Task<AuthResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var user = await _identityService.FindByEmailAsync(request.Email);
        
        // Don't reveal if user exists or not (security best practice)
        if (user == null)
        {
            return new AuthResponse
            {
                Success = true,
                Message = "If an account with that email exists, a password reset link has been sent"
            };
        }

        var token = await _identityService.GeneratePasswordResetTokenAsync(request.Email);
        var appUrl = _configuration.GetAppUrl();
        var resetLink = $"{appUrl}/reset-password?email={Uri.EscapeDataString(request.Email)}&token={Uri.EscapeDataString(token)}";
        
        await _emailService.SendPasswordResetAsync(user.Email, resetLink);

        return new AuthResponse
        {
            Success = true,
            Message = "If an account with that email exists, a password reset link has been sent"
        };
    }

    public async Task<AuthResponse> ResetPasswordAsync(ResetPasswordRequest request)
    {
        // Validate confirm password and password rules
        if (request.NewPassword != request.ConfirmPassword)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Passwords do not match."
            };
        }

        var pwdValid = ValidatePassword(request.NewPassword, out var pwdMessage);
        if (!pwdValid)
        {
            return new AuthResponse
            {
                Success = false,
                Message = pwdMessage
            };
        }

        var (success, errors) = await _identityService.ResetPasswordAsync(
            request.Email,
            request.Token,
            request.NewPassword);

        if (success)
        {
            var user = await _identityService.FindByEmailAsync(request.Email);
            if (user != null)
            {
                var userDisplay = GetDisplayName(user);
                await _userLogService.LogAsync(new CreateUserLogRequest
                {
                    UserId = user.Id,
                    UserName = userDisplay,
                    Action = AuditAction.PasswordReset,
                    EntityName = "User",
                    EntityId = user.Id.ToString()
                });
            }
        }

        return new AuthResponse
        {
            Success = success,
            Message = success ? "Password reset successfully" : string.Join(", ", errors)
        };
    }

    public async Task<AuthResponse> ChangePasswordAsync(Guid userId, ChangePasswordRequest request)
    {
        // Validate confirm password and password rules
        if (request.NewPassword != request.ConfirmPassword)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Passwords do not match."
            };
        }

        var pwdValid = ValidatePassword(request.NewPassword, out var pwdMessage);
        if (!pwdValid)
        {
            return new AuthResponse
            {
                Success = false,
                Message = pwdMessage
            };
        }

        var (success, errors) = await _identityService.ChangePasswordAsync(
            userId,
            request.CurrentPassword,
            request.NewPassword);

        if (success)
        {
            var user = await _identityService.FindByIdAsync(userId);
            if (user != null)
            {
                var userDisplay = GetDisplayName(user);
                await _userLogService.LogAsync(new CreateUserLogRequest
                {
                    UserId = user.Id,
                    UserName = userDisplay,
                    Action = AuditAction.PasswordChanged,
                    EntityName = "User",
                    EntityId = user.Id.ToString()
                });
            }
        }

        return new AuthResponse
        {
            Success = success,
            Message = success ? "Password changed successfully" : string.Join(", ", errors)
        };
    }

    private bool ValidatePassword(string password, out string message)
    {
        message = string.Empty;
        if (string.IsNullOrWhiteSpace(password))
        {
            message = "Password is required.";
            return false;
        }

        if (password.Length < 8)
        {
            message = "Password must be at least 8 characters long.";
            return false;
        }

        if (password.Length > 128)
        {
            message = "Password must not exceed 128 characters.";
            return false;
        }

        if (!Regex.IsMatch(password, @"[A-Z]"))
        {
            message = "Password must contain at least one uppercase letter.";
            return false;
        }

        if (!Regex.IsMatch(password, @"[a-z]"))
        {
            message = "Password must contain at least one lowercase letter.";
            return false;
        }

        if (!Regex.IsMatch(password, @"[0-9]"))
        {
            message = "Password must contain at least one digit.";
            return false;
        }

        if (!Regex.IsMatch(password, @"[^a-zA-Z0-9]"))
        {
            message = "Password must contain at least one special character.";
            return false;
        }

        return true;
    }

    private static string GetDisplayName(Domain.Entities.User user)
    {
        var first = user.FirstName?.Trim();
        var last = user.LastName?.Trim();
        var full = string.IsNullOrEmpty(first) && string.IsNullOrEmpty(last) ? null : $"{first} {last}".Trim();
        return string.IsNullOrEmpty(full) ? user.Email ?? "Unknown" : full;
    }

    #endregion
}
