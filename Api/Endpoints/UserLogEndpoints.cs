using Api.Authorization;
using Application.Features.UserLogs;
using Application.Services;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Constants;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Common;
using Domain.Entities;
using System.Security.Claims;

namespace Api.Endpoints;

public static class UserLogEndpoints
{
    public static void MapUserLogEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/logs").WithTags("Logs");

        // Admin endpoint - requires SystemAudit permission
        group.MapGet("/", async (
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize,
            [FromQuery] Guid? userId,
            [FromQuery] string? entityName,
            [FromQuery] string? entityId,
            [FromServices] IUserLogService userLogService) =>
        {
            var logs = await userLogService.GetLogsAsync(pageNumber ?? 1, pageSize ?? 20, userId, entityName, entityId);
            return Results.Ok(logs);
        })
        .RequireAuthorization()
        .WithMetadata(new RequirePermissionAttribute(Permissions.SystemAudit));

        // User endpoint - allows users to view their own logs
        group.MapGet("/my", async (
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize,
            HttpContext httpContext,
            [FromServices] IUserLogService userLogService) =>
        {
            var userIdString = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Results.Unauthorized();
            }

            var logs = await userLogService.GetLogsAsync(pageNumber ?? 1, pageSize ?? 20, userId, null, null);
            return Results.Ok(logs);
        })
        .WithName("GetMyLogs")
        .WithSummary("Get current user's activity logs")
        .Produces<PaginatedList<UserLogDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .RequireAuthorization();
    }
}
