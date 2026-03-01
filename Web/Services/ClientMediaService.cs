using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web.Models.Media;

namespace Web.Services;

public interface IMediaService
{
    Task<MediaDto> UploadMediaAsync(Guid entityId, string entityType, Stream fileStream, string fileName, string contentType, string collectionName = "default");
    Task<IEnumerable<MediaDto>> GetMediaForEntityAsync(Guid entityId, string entityType, string? collectionName = null);
    Task DeleteMediaAsync(Guid mediaId);
    string GetMediaUrl(MediaDto media);
}

public class ClientMediaService : IMediaService
{
    private readonly HttpClient _httpClient;

    public ClientMediaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<MediaDto> UploadMediaAsync(Guid entityId, string entityType, Stream fileStream, string fileName, string contentType, string collectionName = "default")
    {
        var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        content.Add(fileContent, "file", fileName);

        var response = await _httpClient.PostAsync($"api/media/{entityType}/{entityId}?collectionName={collectionName}", content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<MediaDto>() ?? throw new Exception("Failed to upload media");
    }

    public async Task<IEnumerable<MediaDto>> GetMediaForEntityAsync(Guid entityId, string entityType, string? collectionName = null)
    {
        var url = $"api/media/{entityType}/{entityId}";
        if (!string.IsNullOrEmpty(collectionName))
        {
            url += $"?collectionName={collectionName}";
        }
        return await _httpClient.GetFromJsonAsync<IEnumerable<MediaDto>>(url) ?? Enumerable.Empty<MediaDto>();
    }

    public async Task DeleteMediaAsync(Guid mediaId)
    {
        var response = await _httpClient.DeleteAsync($"api/media/{mediaId}");
        response.EnsureSuccessStatusCode();
    }

    public string GetMediaUrl(MediaDto media)
    {
        if (media == null) return string.Empty;

        if (media.Path.StartsWith("http", StringComparison.OrdinalIgnoreCase)) return media.Path;

        var path = media.Path.Replace("\\", "/").TrimStart('/');
        return new Uri(_httpClient.BaseAddress!, path).ToString();
    }
}
