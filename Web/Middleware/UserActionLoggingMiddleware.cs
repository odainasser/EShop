using System.Text;
using Application.Features.UserLogs;
using Application.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Web.Middleware;

public class UserActionLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UserActionLoggingMiddleware> _logger;

    public UserActionLoggingMiddleware(RequestDelegate next, ILogger<UserActionLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IUserLogService userLogService)
    {
        try
        {
            var path = context.Request.Path.HasValue ? context.Request.Path.Value! : string.Empty;

            // Skip static files and framework endpoints
            if (path.StartsWith("/_framework") || path.StartsWith("/favicon.ico") || path.StartsWith("/css") || path.StartsWith("/js") || path.StartsWith("/lib"))
            {
                await _next(context);
                return;
            }

            // Capture basic request info
            var method = context.Request.Method;
            var user = context.User;
            Guid userId = Guid.Empty;
            string userName = "Anonymous";

            if (user?.Identity?.IsAuthenticated == true)
            {
                var idClaim = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var nameClaim = user.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? user.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                if (!string.IsNullOrEmpty(idClaim) && Guid.TryParse(idClaim, out var parsed))
                    userId = parsed;
                userName = nameClaim ?? user.Identity?.Name ?? "Unknown";
            }

            // Determine action type
            AuditAction action = method.ToUpperInvariant() switch
            {
                "POST" => AuditAction.Created,
                "PUT" => AuditAction.Updated,
                "PATCH" => AuditAction.Updated,
                "DELETE" => AuditAction.Deleted,
                _ => AuditAction.Created // fallback, use Created/Accessed; default to Created for now
            };

            // Call next middleware and capture response status
            await _next(context);

            // Best-effort: don't log for health checks
            if (path.Contains("/health") || path.Contains("/metrics"))
                return;

            // Log asynchronously (don't block response)
            _ = Task.Run(async () =>
            {
                try
                {
                    await userLogService.LogAsync(new CreateUserLogRequest
                    {
                        UserId = userId == Guid.Empty ? Guid.Empty : userId,
                        UserName = userName,
                        Action = action,
                        EntityName = context.Request.Path.HasValue ? context.Request.Path.Value! : null,
                        EntityId = context.Request.Query.ContainsKey("id") ? context.Request.Query["id"].ToString() : null
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex, "Failed to write user action log for path {Path}", path);
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "Error in UserActionLoggingMiddleware");
            await _next(context);
        }
    }
}
