using System.Net.Http.Json;
using Web.Models.Settings;

namespace Web.Services;

public interface ISystemSettingService
{
    Task<SettingsDto> GetSettingsAsync(CancellationToken cancellationToken = default);
    Task UpdateSettingsAsync(SettingsDto settings, CancellationToken cancellationToken = default);
    Task<EmailSettingsDto> GetEmailSettingsAsync(CancellationToken cancellationToken = default);
    Task UpdateEmailSettingsAsync(EmailSettingsDto settings, CancellationToken cancellationToken = default);
}

public class ClientSystemSettingService : ISystemSettingService
{
    private readonly HttpClient _httpClient;

    public ClientSystemSettingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<SettingsDto> GetSettingsAsync(CancellationToken cancellationToken = default)
    {
        // Try to get full admin settings first (requires authentication + permission)
        try
        {
            var adminSettings = await _httpClient.GetFromJsonAsync<SettingsDto>("api/settings/admin", cancellationToken);
            if (adminSettings != null)
            {
                return adminSettings;
            }
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Forbidden || 
                                               ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            // User doesn't have permission - fall back to public endpoint
        }
        catch
        {
            // Other error - fall back to public endpoint
        }

        // Fall back to public endpoint (theme settings only)
        return await _httpClient.GetFromJsonAsync<SettingsDto>("api/settings", cancellationToken) 
               ?? new SettingsDto();
    }

    public async Task UpdateSettingsAsync(SettingsDto settings, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync("api/settings", settings, cancellationToken);
        await response.HandleErrorAsync();
    }

    public async Task<EmailSettingsDto> GetEmailSettingsAsync(CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<EmailSettingsDto>("api/settings/email", cancellationToken) 
               ?? new EmailSettingsDto();
    }

    public async Task UpdateEmailSettingsAsync(EmailSettingsDto settings, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync("api/settings/email", settings, cancellationToken);
        await response.HandleErrorAsync();
    }
}
