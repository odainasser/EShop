namespace Eshop.Domain.Common;

public static class ExtensionMethods
{

    public static HttpContext Clone(this HttpContext originalContext)
    {
        var clonedContext = new DefaultHttpContext
        {
            Request =
            {
                // Copy Request properties
                Scheme = originalContext.Request.Scheme,
                Host = originalContext.Request.Host,
                Path = originalContext.Request.Path,
                QueryString = originalContext.Request.QueryString,
                Method = originalContext.Request.Method,
                ContentType = originalContext.Request.ContentType
            }
        };

        // Copy headers
        foreach (var header in originalContext.Request.Headers)
        {
            clonedContext.Request.Headers[header.Key] = header.Value;
        }

        // Copy cookies
        foreach (var cookie in originalContext.Request.Cookies)
        {
            clonedContext.Request.Cookies.Append(new KeyValuePair<string, string>(cookie.Key, cookie.Value));
        }

        // Copy user (claims and identity information)
        if (originalContext.User != null)
        {
            clonedContext.User = originalContext.User;
        }

        // Copy items (usually used to store state during request processing)
        foreach (var item in originalContext.Items)
        {
            clonedContext.Items[item.Key] = item.Value;
        }

        // You can also copy other properties like Response headers, etc., if needed

        return clonedContext;
    }

    public static HttpContext CreateNew(this HttpContext originalContext,
        IServiceProvider serviceProvider)
    {
        var clonedContext = new DefaultHttpContext
        {
            Request =
            {
                // Copy Request properties
                Scheme = "https",
                Host = new HostString("localhost", 5001),
                Path = new PathString("/test"),
                QueryString = new QueryString(""),
                Method = "Get",
                ContentType = "html",
            },
            RequestServices = serviceProvider.CreateScope().ServiceProvider
        };

        return clonedContext;
    }

}
