namespace Eshop.Domain.Common;

public static class ExtraAuthConstants
{
    public const int UserIdSize = 256;
    public const int RoleNameSize = 100;
}

[Flags]
public enum PaidForModules
{
    None = 0
}
