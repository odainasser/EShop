using Application.Features.Auth;

namespace Application.Services;

public interface IAuthService
{
    // Authentication
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    
    // Email Confirmation
    Task<AuthResponse> SendEmailConfirmationAsync(string email);
    Task<AuthResponse> ConfirmEmailAsync(ConfirmEmailRequest request);
    
    // Password Management
    Task<AuthResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<AuthResponse> ResetPasswordAsync(ResetPasswordRequest request);
    Task<AuthResponse> ChangePasswordAsync(Guid userId, ChangePasswordRequest request);
}
