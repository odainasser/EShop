using Application.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class MediaEndpoints
{
    public static void MapMediaEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/media").WithTags("Media");

        group.MapPost("/{entityType}/{entityId}", async (
            string entityType,
            Guid entityId,
            IFormFile file,
            [FromQuery] string? collectionName,
            [FromServices] IMediaService mediaService) =>
        {
            if (file == null || file.Length == 0)
                return Results.BadRequest("No file uploaded.");

            if (!Enum.TryParse<EntityType>(entityType, true, out var parsedEntityType))
                parsedEntityType = EntityType.Unknown;

            using var stream = file.OpenReadStream();
            var media = await mediaService.UploadMediaAsync(
                entityId,
                parsedEntityType,
                stream,
                file.FileName,
                file.ContentType,
                collectionName ?? "default");

            return Results.Ok(media);
        })
        .DisableAntiforgery(); // Disable antiforgery for API upload

        group.MapGet("/{entityType}/{entityId}", async (
            string entityType,
            Guid entityId,
            [FromQuery] string? collectionName,
            [FromServices] IMediaService mediaService) =>
        {
            if (!Enum.TryParse<EntityType>(entityType, true, out var parsedEntityType))
                parsedEntityType = EntityType.Unknown;

            var mediaList = await mediaService.GetMediaForEntityAsync(entityId, parsedEntityType, collectionName);
            return Results.Ok(mediaList);
        });

        group.MapDelete("/{id:guid}", async (
            Guid id,
            [FromServices] IMediaService mediaService) =>
        {
            await mediaService.DeleteMediaAsync(id);
            return Results.NoContent();
        });
    }
}
