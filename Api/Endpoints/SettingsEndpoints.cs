using Api.Authorization;
using Application.Features.Settings;
using Application.Services;
using Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class SettingsEndpoints
{
    public static void MapSettingsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/settings")
            .WithTags("Settings");

        // Public endpoint for theme settings (no auth required)
        group.MapGet("", async (
            [FromServices] ISystemSettingService settingService,
            CancellationToken cancellationToken) =>
        {
            var settings = await settingService.GetSettingsAsync(cancellationToken);
            // Return only theme-related settings for public access
            return Results.Ok(new
            {
                settings.PrimaryColor,
                settings.CompanyNameEn,
                settings.CompanyNameAr,
                settings.LogoData
            });
        })
        .WithName("GetSettings")
        .WithSummary("Get public theme settings - No auth required")
        .Produces(StatusCodes.Status200OK)
        .AllowAnonymous();

        // Admin endpoint for full settings (auth required)
        group.MapGet("/admin", async (
            [FromServices] ISystemSettingService settingService,
            CancellationToken cancellationToken) =>
        {
            var settings = await settingService.GetSettingsAsync(cancellationToken);
            return Results.Ok(settings);
        })
        .WithName("GetAdminSettings")
        .WithSummary("Get full settings - Requires system.settings permission")
        .Produces<SettingsDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status403Forbidden)
        .RequireAuthorization()
        .WithMetadata(new RequirePermissionAttribute(Permissions.SystemSettings));

        group.MapPut("", async (
            [FromBody] SettingsDto request,
            [FromServices] ISystemSettingService settingService,
            CancellationToken cancellationToken) =>
        {
            await settingService.UpdateSettingsAsync(request, cancellationToken);
            return Results.Ok();
        })
        .WithName("UpdateSettings")
        .WithSummary("Update combined settings - Requires system.settings permission")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status403Forbidden)
        .RequireAuthorization()
        .WithMetadata(new RequirePermissionAttribute(Permissions.SystemSettings));

        // Legacy email endpoints
        group.MapGet("/email", async (
            [FromServices] ISystemSettingService settingService,
            CancellationToken cancellationToken) =>
        {
            var settings = await settingService.GetEmailSettingsAsync(cancellationToken);
            return Results.Ok(settings);
        })
        .WithName("GetEmailSettings")
        .WithSummary("Get email settings - Requires system.settings permission")
        .Produces<EmailSettingsDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status403Forbidden)
        .RequireAuthorization()
        .WithMetadata(new RequirePermissionAttribute(Permissions.SystemSettings));

        group.MapPut("/email", async (
            [FromBody] EmailSettingsDto request,
            [FromServices] ISystemSettingService settingService,
            CancellationToken cancellationToken) =>
        {
            await settingService.UpdateEmailSettingsAsync(request, cancellationToken);
            return Results.Ok();
        })
        .WithName("UpdateEmailSettings")
        .WithSummary("Update email settings - Requires system.settings permission")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status403Forbidden)
        .RequireAuthorization()
        .WithMetadata(new RequirePermissionAttribute(Permissions.SystemSettings));
    }
}
