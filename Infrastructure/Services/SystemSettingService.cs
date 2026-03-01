using Application.Common.Interfaces;
using Application.Features.Settings;
using Application.Services;
using Domain.Constants;
using Domain.Entities;
using Domain.Repositories;
using Application.Features.UserLogs;
using Domain.Enums;

namespace Infrastructure.Services;

public class SystemSettingService : ISystemSettingService
{
    private readonly IRepository<SystemSetting> _repository;
    private readonly IUserLogService _userLogService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMediaService _mediaService;

    // Use fixed ID for settings media entity
    private static readonly Guid SettingsMediaEntityId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public SystemSettingService(
        IRepository<SystemSetting> repository,
        IUserLogService userLogService,
        ICurrentUserService currentUserService,
        IMediaService mediaService)
    {
        _repository = repository;
        _userLogService = userLogService;
        _currentUserService = currentUserService;
        _mediaService = mediaService;
    }

    public async Task<SettingsDto> GetSettingsAsync(CancellationToken cancellationToken = default)
    {
        var groups = new[] { "Email", "Theme", "General" };
        var settings = await _repository.FindAsync(s => 
            s.Group != null && groups.Contains(s.Group), 
            cancellationToken);

        var settingsDict = settings.ToDictionary(s => s.Key, s => s.Value, StringComparer.OrdinalIgnoreCase);

        int smtpPort = 587;
        if (settingsDict.TryGetValue(SettingKeys.EmailSmtpPort, out var smtpPortRaw))
            int.TryParse(smtpPortRaw, out smtpPort);

        bool enableSsl = true;
        if (settingsDict.TryGetValue(SettingKeys.EmailEnableSsl, out var enableSslRaw))
            bool.TryParse(enableSslRaw, out enableSsl);

        var dto = new SettingsDto
        {
            // Email
            SmtpHost = settingsDict.GetValueOrDefault(SettingKeys.EmailSmtpHost, ""),
            SmtpPort = smtpPort,
            SmtpUsername = settingsDict.GetValueOrDefault(SettingKeys.EmailSmtpUsername, ""),
            SmtpPassword = settingsDict.GetValueOrDefault(SettingKeys.EmailSmtpPassword, ""),
            FromAddress = settingsDict.GetValueOrDefault(SettingKeys.EmailFromAddress, ""),
            FromNameEn = settingsDict.GetValueOrDefault(SettingKeys.EmailFromNameEn, ""),
            FromNameAr = settingsDict.GetValueOrDefault(SettingKeys.EmailFromNameAr, ""),
            EnableSsl = enableSsl,

            // Theme
            PrimaryColor = settingsDict.GetValueOrDefault(SettingKeys.ThemePrimaryColor, "#e2c675"),
            LogoData = settingsDict.GetValueOrDefault(SettingKeys.ThemeLogoData, ""),

            // Company
            CompanyNameEn = settingsDict.GetValueOrDefault(SettingKeys.CompanyNameEn, ""),
            CompanyNameAr = settingsDict.GetValueOrDefault(SettingKeys.CompanyNameAr, "")
        };

        // If logo data is not set in settings, attempt to load from media store for system settings entity
        try
        {
            if (string.IsNullOrEmpty(dto.LogoData))
            {
                var mediaList = await _mediaService.GetMediaForEntityAsync(SettingsMediaEntityId, EntityType.SystemSetting, "logo");
                var logo = mediaList.FirstOrDefault();
                if (logo != null)
                {
                    dto.LogoData = _mediaService.GetMediaUrl(logo);
                }
            }
        }
        catch
        {
            // ignore media load errors
        }

        return dto;
    }

    public async Task UpdateSettingsAsync(SettingsDto settings, CancellationToken cancellationToken = default)
    {
        // Email
        await UpdateSettingAsync(SettingKeys.EmailSmtpHost, settings.SmtpHost, "Email", cancellationToken);
        await UpdateSettingAsync(SettingKeys.EmailSmtpPort, settings.SmtpPort.ToString(), "Email", cancellationToken);
        await UpdateSettingAsync(SettingKeys.EmailSmtpUsername, settings.SmtpUsername, "Email", cancellationToken);
        await UpdateSettingAsync(SettingKeys.EmailSmtpPassword, settings.SmtpPassword, "Email", cancellationToken);
        await UpdateSettingAsync(SettingKeys.EmailFromAddress, settings.FromAddress, "Email", cancellationToken);
        await UpdateSettingAsync(SettingKeys.EmailFromNameEn, settings.FromNameEn, "Email", cancellationToken);
        await UpdateSettingAsync(SettingKeys.EmailFromNameAr, settings.FromNameAr, "Email", cancellationToken);
        await UpdateSettingAsync(SettingKeys.EmailEnableSsl, settings.EnableSsl.ToString(), "Email", cancellationToken);

        // Theme
        await UpdateSettingAsync(SettingKeys.ThemePrimaryColor, settings.PrimaryColor, "Theme", cancellationToken);
        await UpdateSettingAsync(SettingKeys.ThemeLogoData, settings.LogoData ?? string.Empty, "Theme", cancellationToken);

        // Company
        await UpdateSettingAsync(SettingKeys.CompanyNameEn, settings.CompanyNameEn ?? string.Empty, "General", cancellationToken);
        await UpdateSettingAsync(SettingKeys.CompanyNameAr, settings.CompanyNameAr ?? string.Empty, "General", cancellationToken);

        await _repository.SaveChangesAsync(cancellationToken);

        // Log action
        var (currentUserId, currentUserName) = await _currentUserService.GetCurrentUserAsync();
        if (currentUserId != Guid.Empty)
        {
            await _userLogService.LogAsync(new CreateUserLogRequest
            {
                UserId = currentUserId,
                UserName = currentUserName,
                Action = AuditAction.Updated,
                EntityName = "SystemSetting",
                EntityId = "Settings"
            });
        }
    }

    public async Task<EmailSettingsDto> GetEmailSettingsAsync(CancellationToken cancellationToken = default)
    {
        var settings = await _repository.FindAsync(s => string.Equals(s.Group, "Email", StringComparison.OrdinalIgnoreCase), cancellationToken);
        var settingsDict = settings.ToDictionary(s => s.Key, s => s.Value, StringComparer.OrdinalIgnoreCase);

        int smtpPort = 587;
        if (settingsDict.TryGetValue(SettingKeys.EmailSmtpPort, out var smtpPortRaw))
            int.TryParse(smtpPortRaw, out smtpPort);

        bool enableSsl = true;
        if (settingsDict.TryGetValue(SettingKeys.EmailEnableSsl, out var enableSslRaw))
            bool.TryParse(enableSslRaw, out enableSsl);

        return new EmailSettingsDto
        {
            SmtpHost = settingsDict.GetValueOrDefault(SettingKeys.EmailSmtpHost, ""),
            SmtpPort = smtpPort,
            SmtpUsername = settingsDict.GetValueOrDefault(SettingKeys.EmailSmtpUsername, ""),
            SmtpPassword = settingsDict.GetValueOrDefault(SettingKeys.EmailSmtpPassword, ""),
            FromAddress = settingsDict.GetValueOrDefault(SettingKeys.EmailFromAddress, ""),
            FromNameEn = settingsDict.GetValueOrDefault(SettingKeys.EmailFromNameEn, ""),
            FromNameAr = settingsDict.GetValueOrDefault(SettingKeys.EmailFromNameAr, ""),
            EnableSsl = enableSsl
        };
    }

    public async Task UpdateEmailSettingsAsync(EmailSettingsDto settings, CancellationToken cancellationToken = default)
    {
        await UpdateSettingAsync(SettingKeys.EmailSmtpHost, settings.SmtpHost, "Email", cancellationToken);
        await UpdateSettingAsync(SettingKeys.EmailSmtpPort, settings.SmtpPort.ToString(), "Email", cancellationToken);
        await UpdateSettingAsync(SettingKeys.EmailSmtpUsername, settings.SmtpUsername, "Email", cancellationToken);
        await UpdateSettingAsync(SettingKeys.EmailSmtpPassword, settings.SmtpPassword, "Email", cancellationToken);
        await UpdateSettingAsync(SettingKeys.EmailFromAddress, settings.FromAddress, "Email", cancellationToken);
        await UpdateSettingAsync(SettingKeys.EmailFromNameEn, settings.FromNameEn, "Email", cancellationToken);
        await UpdateSettingAsync(SettingKeys.EmailFromNameAr, settings.FromNameAr, "Email", cancellationToken);
        await UpdateSettingAsync(SettingKeys.EmailEnableSsl, settings.EnableSsl.ToString(), "Email", cancellationToken);

        await _repository.SaveChangesAsync(cancellationToken);

        var (currentUserId, currentUserName) = await _currentUserService.GetCurrentUserAsync();
        if (currentUserId != Guid.Empty)
        {
            await _userLogService.LogAsync(new CreateUserLogRequest
            {
                UserId = currentUserId,
                UserName = currentUserName,
                Action = AuditAction.Updated,
                EntityName = "SystemSetting",
                EntityId = "EmailSettings"
            });
        }
    }

    private async Task UpdateSettingAsync(string key, string value, string group, CancellationToken cancellationToken)
    {
        var existingSettings = await _repository.FindAsync(s => s.Key == key, cancellationToken);
        var setting = existingSettings.FirstOrDefault();

        if (setting == null)
        {
            setting = new SystemSetting
            {
                Key = key,
                Value = value ?? string.Empty,
                Group = group
            };
            await _repository.AddAsync(setting, cancellationToken);
        }
        else
        {
            setting.Value = value ?? string.Empty;
            await _repository.UpdateAsync(setting, cancellationToken);
        }
    }
}
