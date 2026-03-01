using Web.Models.Auth;

namespace Web.Services;

public interface IAuthenticationService
{
    // Authentication
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task LogoutAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<UserDto?> GetCurrentUserAsync();
    
    // Email Confirmation
    Task<AuthResponse> SendEmailConfirmationAsync(string email);
    Task<AuthResponse> ConfirmEmailAsync(ConfirmEmailRequest request);
    
    // Password Management
    Task<AuthResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<AuthResponse> ResetPasswordAsync(ResetPasswordRequest request);
    Task<AuthResponse> ChangePasswordAsync(ChangePasswordRequest request);
    
    // Profile Management
    Task<AuthResponse> UpdateProfileAsync(object profileData);
}
