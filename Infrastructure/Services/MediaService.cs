using Application.Services;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using Application.Common.Interfaces;

namespace Infrastructure.Services;

public class MediaService : IMediaService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly IAppConfiguration _appConfig;

    public MediaService(ApplicationDbContext context, IWebHostEnvironment environment, IAppConfiguration appConfig)
    {
        _context = context;
        _environment = environment;
        _appConfig = appConfig;
    }

    public async Task<Media> UploadMediaAsync(Guid entityId, EntityType entityType, Stream fileStream, string fileName, string contentType, string collectionName = "default")
    {
        if (string.IsNullOrWhiteSpace(_environment.WebRootPath))
        {
            throw new InvalidOperationException("WebRootPath is not configured. Ensure the API project has a wwwroot folder.");
        }

        var entityDir = entityType.ToString().ToLower();
        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", entityDir, entityId.ToString());
        
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.CopyToAsync(stream);
        }

        var media = new Media
        {
            Id = Guid.NewGuid(),
            EntityId = entityId,
            EntityType = entityType,
            CollectionName = collectionName,
            Name = Path.GetFileNameWithoutExtension(fileName),
            FileName = uniqueFileName,
            MimeType = contentType,
            Disk = "local",
            Path = Path.Combine("uploads", entityDir, entityId.ToString(), uniqueFileName).Replace("\\", "/"),
            Size = fileStream.Length,
            Order = 0
        };

        _context.Media.Add(media);
        await _context.SaveChangesAsync();

        return media;
    }

    public async Task<IEnumerable<Media>> GetMediaForEntityAsync(Guid entityId, EntityType entityType, string? collectionName = null)
    {
        var query = _context.Media.Where(m => m.EntityId == entityId && m.EntityType == entityType);
        
        if (!string.IsNullOrEmpty(collectionName))
        {
            query = query.Where(m => m.CollectionName == collectionName);
        }

        return await query.OrderBy(m => m.Order).ThenBy(m => m.CreatedAt).ToListAsync();
    }

    public async Task DeleteMediaAsync(Guid mediaId)
    {
        var media = await _context.Media.FindAsync(mediaId);
        if (media != null)
        {
            if (!string.IsNullOrWhiteSpace(_environment.WebRootPath))
            {
                var filePath = Path.Combine(_environment.WebRootPath, media.Path);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            _context.Media.Remove(media);
            await _context.SaveChangesAsync();
        }
    }

    public string GetMediaUrl(Media media)
    {
        if (media.Path.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            return media.Path;

        var baseUrl = _appConfig.GetAppUrl().TrimEnd('/');
        // Ensure path starts with a forward slash for proper URL construction
        var trimmedPath = media.Path.TrimStart('/');
        return $"{baseUrl}/{trimmedPath}";
    }
}
