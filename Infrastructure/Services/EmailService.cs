using System.Net;
using System.Net.Mail;
using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IRepository<SystemSetting> _settingsRepository;

    public EmailService(ILogger<EmailService> logger, IRepository<SystemSetting> settingsRepository)
    {
        _logger = logger;
        _settingsRepository = settingsRepository;
    }

    public async Task SendEmailConfirmationAsync(string email, string confirmationLink)
    {
        var companyName = await GetCompanyNameAsync();
        var subject = $"Confirm your email - {companyName}";
        var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2 style='color: #8B5CF6;'>Email Confirmation</h2>
                    <p>Thank you for registering with {companyName}!</p>
                    <p>Please confirm your email address by clicking the button below:</p>
                    <div style='margin: 30px 0;'>
                        <a href='{confirmationLink}' 
                           style='display: inline-block; padding: 12px 30px; background-color: #8B5CF6; color: white; text-decoration: none; border-radius: 5px; font-weight: bold;'>
                            Confirm Email
                        </a>
                    </div>
                    <p style='color: #666; font-size: 14px'>If the button doesn't work, copy and paste this link into your browser:</p>
                    <p style='word-break: break-all; color: #8B5CF6; font-size: 12px;'>{confirmationLink}</p>
                    <p style='color: #666; margin-top: 30px'>If you didn't create an account, please ignore this email.</p>
                    <p>Best regards,<br/>{companyName} Team</p>
                </div>
            </body>
            </html>
        ";

        await SendEmailAsync(email, subject, body);
    }

    public async Task SendPasswordResetAsync(string email, string resetLink)
    {
        var companyName = await GetCompanyNameAsync();
        var subject = $"Password Reset - {companyName}";
        var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2 style='color: #8B5CF6;'>Password Reset Request</h2>
                    <p>We received a request to reset your password.</p>
                    <p>Click the button below to reset your password:</p>
                    <div style='margin: 30px 0;'>
                        <a href='{resetLink}' 
                           style='display: inline-block; padding: 12px 30px; background-color: #8B5CF6; color: white; text-decoration: none; border-radius: 5px; font-weight: bold;'>
                            Reset Password
                        </a>
                    </div>
                    <p style='color: #666; font-size: 14px'>If the button doesn't work, copy and paste this link into your browser:</p>
                    <p style='word-break: break-all; color: #8B5CF6; font-size: 12px;'>{resetLink}</p>
                    <p style='color: #e74c3c; margin-top: 20px;'><strong>This link will expire in 24 hours.</strong></p>
                    <p style='color: #666; margin-top: 30px'>If you didn't request a password reset, please ignore this email or contact support if you have concerns.</p>
                    <p>Best regards,<br/>{companyName} Team</p>
                </div>
            </body>
            </html>
        ";

        await SendEmailAsync(email, subject, body);
    }

    public async Task SendTwoFactorCodeAsync(string email, string code)
    {
        var companyName = await GetCompanyNameAsync();
        var subject = $"Two-Factor Authentication Code - {companyName}";
        var body = $@"
            <html>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2 style='color: #8B5CF6;'>Your 2FA Code</h2>
                    <p>Your two-factor authentication code is:</p>
                    <h1 style='color: #8B5CF6; letter-spacing: 5px; text-align: center; font-size: 36px; margin: 30px 0;'>{code}</h1>
                    <p style='color: #e74c3c;'><strong>This code will expire in 5 minutes.</strong></p>
                    <p style='color: #666; margin-top: 30px'>If you didn't request this code, please secure your account immediately.</p>
                    <p>Best regards,<br/>{companyName} Team</p>
                </div>
            </body>
            </html>
        ";

        await SendEmailAsync(email, subject, body);
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            var settings = await _settingsRepository.FindAsync(s => s.Group == "Email");
            var settingsDict = settings.ToDictionary(s => s.Key, s => s.Value, StringComparer.OrdinalIgnoreCase);

            if (!settingsDict.ContainsKey(SettingKeys.EmailSmtpHost) || string.IsNullOrEmpty(settingsDict[SettingKeys.EmailSmtpHost]))
            {
                _logger.LogWarning("Email settings not configured. Email to {To} not sent", to);
                return;
            }

            var host = settingsDict[SettingKeys.EmailSmtpHost];
            var port = int.Parse(settingsDict.GetValueOrDefault(SettingKeys.EmailSmtpPort, "587"));
            var username = settingsDict.GetValueOrDefault(SettingKeys.EmailSmtpUsername, "");
            var password = settingsDict.GetValueOrDefault(SettingKeys.EmailSmtpPassword, "");
            var fromAddress = settingsDict.GetValueOrDefault(SettingKeys.EmailFromAddress, "");
            
            // Default to English if available, otherwise check legacy/fallback or default
            var fromName = settingsDict.GetValueOrDefault(SettingKeys.EmailFromNameEn, 
                settingsDict.GetValueOrDefault("Email.FromName", await GetCompanyNameAsync()));
            
            // Parse EnableSsl setting
            bool enableSsl = true;
            var enableSslValue = settingsDict.GetValueOrDefault(SettingKeys.EmailEnableSsl, "true");
            if (!string.IsNullOrWhiteSpace(enableSslValue))
            {
                enableSsl = enableSslValue.Trim().Equals("true", StringComparison.OrdinalIgnoreCase) ||
                           enableSslValue.Trim().Equals("1") ||
                           enableSslValue.Trim().Equals("yes", StringComparison.OrdinalIgnoreCase);
            }

            // Validate required credentials
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("Email credentials not configured. Email to {To} not sent", to);
                return;
            }

            // Use username as fromAddress if not configured
            if (string.IsNullOrWhiteSpace(fromAddress))
            {
                fromAddress = username;
            }

            // Validate email addresses
            if (string.IsNullOrWhiteSpace(to) || !IsValidEmail(to))
            {
                _logger.LogError("Invalid recipient email address: {To}", to);
                return;
            }

            if (!IsValidEmail(fromAddress))
            {
                fromAddress = username;
                if (!IsValidEmail(fromAddress))
                {
                    _logger.LogError("Invalid from email address. Username is also not a valid email");
                    return;
                }
            }

            _logger.LogInformation("Sending email to {To} via {Host}:{Port}", to, host, port);

            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = enableSsl,
                Timeout = 30000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromAddress, fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(new MailAddress(to));

            await client.SendMailAsync(mailMessage);
            _logger.LogInformation("Email sent successfully to {To}", to);
        }
        catch (FormatException formatEx)
        {
            _logger.LogError(formatEx, "Invalid email address format when sending to {To}", to);
        }
        catch (SmtpException smtpEx)
        {
            _logger.LogError(smtpEx, "SMTP error sending email to {To}. Status: {Status}", to, smtpEx.StatusCode);
            
            if (smtpEx.StatusCode == SmtpStatusCode.MustIssueStartTlsFirst || 
                smtpEx.Message.Contains("5.7.0", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogError("Gmail authentication failed. Ensure you are using an App Password, not your regular Gmail password. Generate one at: https://myaccount.google.com/apppasswords");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}", to);
        }
    }

    private async Task<string> GetCompanyNameAsync()
    {
        try
        {
            var settings = await _settingsRepository.FindAsync(s => s.Group == "General");
            var companyNameSetting = settings.FirstOrDefault(s => s.Key == SettingKeys.CompanyNameEn);
            
            // Fallback to legacy key if English is not found
            if (companyNameSetting == null || string.IsNullOrWhiteSpace(companyNameSetting.Value))
            {
                companyNameSetting = settings.FirstOrDefault(s => s.Key == "General.CompanyName");
            }
            
            return !string.IsNullOrWhiteSpace(companyNameSetting?.Value) 
                ? companyNameSetting.Value 
                : "Sharjah Museums Authority";
        }
        catch
        {
            return "Sharjah Museums Authority";
        }
    }

    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email.Trim();
        }
        catch
        {
            return false;
        }
    }
}
