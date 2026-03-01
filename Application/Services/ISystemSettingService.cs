using Application.Features.Settings;

namespace Application.Services;

public interface ISystemSettingService
{
    Task<SettingsDto> GetSettingsAsync(CancellationToken cancellationToken = default);
    Task UpdateSettingsAsync(SettingsDto settings, CancellationToken cancellationToken = default);

    // Backwards compatibility
    Task<EmailSettingsDto> GetEmailSettingsAsync(CancellationToken cancellationToken = default);
    Task UpdateEmailSettingsAsync(EmailSettingsDto settings, CancellationToken cancellationToken = default);
}
