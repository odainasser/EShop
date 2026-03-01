namespace Web.Constants;

public static class Roles
{
    public const string Administrator = "Administrator";
    public const string Client = "Client";

    public static readonly string[] All = 
    {
        Administrator,
        Client
    };

    public static bool IsValid(string role) => All.Contains(role);
}
