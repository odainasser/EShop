namespace Domain.Constants;

public static class SettingKeys
{
    public const string EmailSmtpHost = "Email.SmtpHost";
    public const string EmailSmtpPort = "Email.SmtpPort";
    public const string EmailSmtpUsername = "Email.SmtpUsername";
    public const string EmailSmtpPassword = "Email.SmtpPassword";
    public const string EmailFromAddress = "Email.FromAddress";
    // public const string EmailFromName = "Email.FromName"; // Deprecated
    public const string EmailFromNameEn = "Email.FromNameEn";
    public const string EmailFromNameAr = "Email.FromNameAr";
    public const string EmailEnableSsl = "Email.EnableSsl";

    // Theme settings
    public const string ThemePrimaryColor = "Theme.PrimaryColor";
    public const string ThemeLogoData = "Theme.LogoData"; // base64 or URL

    // Company settings
    // public const string CompanyName = "General.CompanyName"; // Deprecated
    public const string CompanyNameEn = "General.CompanyNameEn";
    public const string CompanyNameAr = "General.CompanyNameAr";
}
