namespace Domain.Enums;

public enum AuditAction
{
    Created = 1,
    Updated = 2,
    Deleted = 3,
    LoggedIn = 4,
    LoggedOut = 5,
    PasswordChanged = 6,
    PasswordReset = 7,
    EmailVerified = 8,
    TwoFactorEnabled = 9,
    TwoFactorDisabled = 10
}
