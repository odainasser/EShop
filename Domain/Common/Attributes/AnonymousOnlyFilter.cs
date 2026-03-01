namespace Eshop.Domain.Common.Attributes;

public sealed class AnonymousOnlyAttribute() : TypeFilterAttribute(typeof(AnonymousOnlyFilter));

public class AnonymousOnlyFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var httpContext = context.HttpContext;
        
        if (httpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new LocalRedirectResult("/");
        }
    }
}