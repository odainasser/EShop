namespace Eshop.Domain.Common;

[Flags]
public enum Permissions
{
    NotSet = 0,
    AccessAll = int.MaxValue
}

public static class PermissionsExtensions
{
    public static IEnumerable<Permissions>? UnpackPermissionsFromString(this string? packedPermissions)
    {
        if (string.IsNullOrEmpty(packedPermissions))
            return null;

        return packedPermissions.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => Enum.TryParse<Permissions>(p.Trim(), out var perm) ? perm : Permissions.NotSet)
            .Where(p => p != Permissions.NotSet);
    }

    public static string PackPermissionsIntoString(this ICollection<Permissions> permissions)
    {
        return string.Join(',', permissions.Select(p => p.ToString()));
    }
}
