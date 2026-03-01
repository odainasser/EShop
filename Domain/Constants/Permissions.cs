namespace Domain.Constants;

public static class Permissions
{
    public const string UsersRead = "users.read";
    public const string UsersWrite = "users.write";
    public const string UsersDelete = "users.delete";

    public const string ClientsRead = "clients.read";
    public const string ClientsWrite = "clients.write";
    public const string ClientsDelete = "clients.delete";

    public const string RolesRead = "roles.read";
    public const string RolesWrite = "roles.write";
    public const string RolesDelete = "roles.delete";

    public const string SystemSettings = "system.settings";
    public const string SystemAudit = "system.audit";

    public const string LookupsRead = "lookups.read";
    public const string LookupsWrite = "lookups.write";
    public const string LookupsDelete = "lookups.delete";

    public static readonly string[] All =
    {
        UsersRead, UsersWrite, UsersDelete,
        ClientsRead, ClientsWrite, ClientsDelete,
        RolesRead, RolesWrite, RolesDelete,
        SystemSettings, SystemAudit,
        LookupsRead, LookupsWrite, LookupsDelete
    };

    public static bool IsValid(string permission) => All.Contains(permission);
}
