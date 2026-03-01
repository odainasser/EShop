using System.Net.Http.Json;
using Web.Models;
using Web.Models.UserLogs;

namespace Web.Services;

public interface IUserLogService
{
    Task<PaginatedList<UserLogDto>> GetLogsAsync(int pageNumber, int pageSize, Guid? userId = null, string? entityName = null, string? entityId = null);
    Task<PaginatedList<UserLogDto>> GetMyLogsAsync(int pageNumber, int pageSize);
}

public class ClientUserLogService : IUserLogService
{
    private readonly HttpClient _httpClient;

    public ClientUserLogService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PaginatedList<UserLogDto>> GetLogsAsync(int pageNumber, int pageSize, Guid? userId = null, string? entityName = null, string? entityId = null)
    {
        var url = $"api/logs?pageNumber={pageNumber}&pageSize={pageSize}";
        if (userId.HasValue)
        {
            url += $"&userId={userId}";
        }
        if (!string.IsNullOrEmpty(entityName))
        {
            url += $"&entityName={Uri.EscapeDataString(entityName)}";
        }
        if (!string.IsNullOrEmpty(entityId))
        {
            url += $"&entityId={Uri.EscapeDataString(entityId)}";
        }
        return await _httpClient.GetFromJsonAsync<PaginatedList<UserLogDto>>(url) 
               ?? new PaginatedList<UserLogDto>(new List<UserLogDto>(), 0, pageNumber, pageSize);
    }

    public async Task<PaginatedList<UserLogDto>> GetMyLogsAsync(int pageNumber, int pageSize)
    {
        var url = $"api/logs/my?pageNumber={pageNumber}&pageSize={pageSize}";
        return await _httpClient.GetFromJsonAsync<PaginatedList<UserLogDto>>(url) 
               ?? new PaginatedList<UserLogDto>(new List<UserLogDto>(), 0, pageNumber, pageSize);
    }
}
