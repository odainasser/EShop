namespace Web.Models.Settings;

public class SettingsDto
{
    // Email settings
    public string SmtpHost { get; set; } = string.Empty;
    public int SmtpPort { get; set; } = 587;
    public string SmtpUsername { get; set; } = string.Empty;
    public string SmtpPassword { get; set; } = string.Empty;
    public string FromAddress { get; set; } = string.Empty;
    public string FromNameEn { get; set; } = string.Empty;
    public string FromNameAr { get; set; } = string.Empty;
    public bool EnableSsl { get; set; } = true;

    // Theme settings
    public string PrimaryColor { get; set; } = "#e2c675";
    public string LogoData { get; set; } = string.Empty;

    // Company settings
    public string CompanyNameEn { get; set; } = string.Empty;
    public string CompanyNameAr { get; set; } = string.Empty;
}

public class EmailSettingsDto
{
    public string SmtpHost { get; set; } = string.Empty;
    public int SmtpPort { get; set; } = 587;
    public string SmtpUsername { get; set; } = string.Empty;
    public string SmtpPassword { get; set; } = string.Empty;
    public string FromAddress { get; set; } = string.Empty;
    public string FromNameEn { get; set; } = string.Empty;
    public string FromNameAr { get; set; } = string.Empty;
    public bool EnableSsl { get; set; } = true;
}
