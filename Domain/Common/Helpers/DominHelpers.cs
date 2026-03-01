namespace Eshop.Domain.Common.Helpers;

public static class DomainHelpers
{
    public static string GetTransCultureName()
    {
        var culture = CultureInfo.CurrentCulture;
        return string.IsNullOrEmpty(culture.Parent?.Name) ? culture.Name : culture.Parent.Name;
    }
}