using System.Net.Http.Json;
using Web.Models.Auth;
using Web.Models.Users;
using Blazored.LocalStorage;

namespace Web.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private const string TokenKey = "authToken";
    private const string UserKey = "currentUser";

    public AuthenticationService(
        HttpClient httpClient,
        ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    #region Authentication

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/login", request);
            
            // Handle 401 Unauthorized specifically
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return new AuthResponse 
                { 
                    Success = false, 
                    Message = "Invalid email or password. Please check your credentials and try again." 
                };
            }

            // Handle validation errors (400 Bad Request)
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                try
                {
                    // First try to read as validation problem details
                    var errorContent = await result.Content.ReadAsStringAsync();
                    Console.WriteLine($"Validation error response: {errorContent}");
                    
                    // Try to parse as AuthResponse first
                    var errorResponse = await result.Content.ReadFromJsonAsync<AuthResponse>();
                    if (errorResponse != null && !string.IsNullOrEmpty(errorResponse.Message))
                    {
                        return errorResponse;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing validation response: {ex.Message}");
                }
                
                return new AuthResponse 
                { 
                    Success = false, 
                    Message = "Invalid request. Please check your email and password format." 
                };
            }

            // Handle other error status codes
            if (!result.IsSuccessStatusCode)
            {
                return new AuthResponse 
                { 
                    Success = false, 
                    Message = $"Login failed with status {result.StatusCode}. Please try again later." 
                };
            }

            var response = await result.Content.ReadFromJsonAsync<AuthResponse>();
            
            if (response != null && response.Success && !string.IsNullOrEmpty(response.Token))
            {
                // ensure DisplayName is set
                if (response.User != null && string.IsNullOrEmpty(response.User.DisplayName))
                {
                    response.User.DisplayName = (!string.IsNullOrEmpty(response.User.FirstName) || !string.IsNullOrEmpty(response.User.LastName))
                        ? $"{response.User.FirstName} {response.User.LastName}".Trim()
                        : response.User.Email;
                }

                await _localStorage.SetItemAsync(TokenKey, response.Token);
                await _localStorage.SetItemAsync(UserKey, response.User);
            }

            return response ?? new AuthResponse { Success = false, Message = "Login failed - no response from server" };
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP Request Exception: {ex.Message}");
            return new AuthResponse 
            { 
                Success = false, 
                Message = "Cannot connect to the API server. Please ensure the API is running." 
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login Exception: {ex.Message}");
            return new AuthResponse { Success = false, Message = "An unexpected error occurred during login" };
        }
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync("api/auth/register", request);
        var response = await result.Content.ReadFromJsonAsync<AuthResponse>();
        
        if (response != null && response.Success && !string.IsNullOrEmpty(response.Token))
        {
            if (response.User != null && string.IsNullOrEmpty(response.User.DisplayName))
            {
                response.User.DisplayName = (!string.IsNullOrEmpty(response.User.FirstName) || !string.IsNullOrEmpty(response.User.LastName))
                    ? $"{response.User.FirstName} {response.User.LastName}".Trim()
                    : response.User.Email;
            }

            await _localStorage.SetItemAsync(TokenKey, response.Token);
            await _localStorage.SetItemAsync(UserKey, response.User);
        }

        return response ?? new AuthResponse { Success = false, Message = "Registration failed" };
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(TokenKey);
        await _localStorage.RemoveItemAsync(UserKey);
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        try
        {
            var token = await _localStorage.GetItemAsync<string>(TokenKey);
            if (string.IsNullOrEmpty(token))
                return false;

            // Ensure stored user exists and is active
            var user = await _localStorage.GetItemAsync<UserDto>(UserKey);
            if (user == null)
            {
                // Cleanup token if user metadata missing
                await _localStorage.RemoveItemAsync(TokenKey);
                return false;
            }

            if (!user.IsActive)
            {
                // If user became inactive, clear storage and treat as not authenticated
                await _localStorage.RemoveItemAsync(TokenKey);
                await _localStorage.RemoveItemAsync(UserKey);
                return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<UserDto?> GetCurrentUserAsync()
    {
        try
        {
            var user = await _localStorage.GetItemAsync<UserDto>(UserKey);
            if (user == null)
            {
                return null;
            }

            // If stored user is inactive, clear storage and return null
            if (!user.IsActive)
            {
                await _localStorage.RemoveItemAsync(TokenKey);
                await _localStorage.RemoveItemAsync(UserKey);
                return null;
            }

            return user;
        }
        catch
        {
            return null;
        }
    }

    #endregion

    #region Email Confirmation

    public async Task<AuthResponse> SendEmailConfirmationAsync(string email)
    {
        // Assuming endpoint exists
        var result = await _httpClient.PostAsJsonAsync("api/auth/send-email-confirmation", email);
        return await result.Content.ReadFromJsonAsync<AuthResponse>() 
               ?? new AuthResponse { Success = false, Message = "Failed to send email" };
    }

    public async Task<AuthResponse> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync("api/auth/confirm-email", request);
        return await result.Content.ReadFromJsonAsync<AuthResponse>()
               ?? new AuthResponse { Success = false, Message = "Failed to confirm email" };
    }

    #endregion

    #region Password Management

    public async Task<AuthResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync("api/auth/forgot-password", request);
        return await result.Content.ReadFromJsonAsync<AuthResponse>()
               ?? new AuthResponse { Success = false, Message = "Request failed" };
    }

    public async Task<AuthResponse> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync("api/auth/reset-password", request);
        return await result.Content.ReadFromJsonAsync<AuthResponse>()
               ?? new AuthResponse { Success = false, Message = "Reset failed" };
    }

    public async Task<AuthResponse> ChangePasswordAsync(ChangePasswordRequest request)
    {
        try
        {
            var token = await _localStorage.GetItemAsync<string>(TokenKey);
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var result = await _httpClient.PostAsJsonAsync("api/auth/change-password", request);
            return await result.Content.ReadFromJsonAsync<AuthResponse>()
                   ?? new AuthResponse { Success = false, Message = "Change password failed" };
        }
        catch (Exception ex)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "An error occurred while changing password"
            };
        }
    }

    #endregion

    #region Profile Management

    public async Task<AuthResponse> UpdateProfileAsync(object profileData)
    {
        try
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "User not authenticated"
                };
            }

            // If profileData is a UserResponse, update DisplayName too
            if (profileData is UserResponse ur)
            {
                var updated = new UserDto
                {
                    Id = ur.Id,
                    Email = ur.Email,
                    FirstName = ur.FirstName,
                    LastName = ur.LastName,
                    DisplayName = string.IsNullOrEmpty(ur.FullName) ? ur.Email : ur.FullName,
                    IsActive = ur.IsActive,
                    Roles = ur.Roles?.Select(r => r.Name).ToList() ?? new List<string>(),
                };

                await _localStorage.SetItemAsync(UserKey, updated);
            }
            else
            {
                await _localStorage.SetItemAsync(UserKey, profileData);
            }

            return new AuthResponse
            {
                Success = true,
                Message = "Profile updated successfully"
            };
        }
        catch (Exception ex)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "An error occurred while updating profile"
            };
        }
    }

    #endregion
}
