using System.Net.Http.Json;
using System.Text.Json;
using Web.Models;
using Web.Models.Lookups;

namespace Web.Services;

public interface ILookupService
{
    Task<PaginatedList<LookupDto>> GetAllLookupsAsync(int pageNumber, int pageSize, string? type = null);
    Task<List<LookupDto>> GetLookupsByTypeAsync(string type);
    Task<List<LookupDto>> GetPublicLookupsByTypeAsync(string type);
    Task<LookupDto?> GetLookupByIdAsync(Guid id);
    Task<LookupDto> CreateLookupAsync(CreateLookupRequest request);
    Task<LookupDto> UpdateLookupAsync(Guid id, UpdateLookupRequest request);
    Task DeleteLookupAsync(Guid id);
    Task<(bool CodeExists, bool NameEnExists, bool NameArExists)> CheckLookupExistsAsync(string code, string nameEn, string nameAr, Guid? excludeLookupId = null);
}

public class ClientLookupService : ILookupService
{
    private readonly HttpClient _httpClient;

    public ClientLookupService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PaginatedList<LookupDto>> GetAllLookupsAsync(int pageNumber, int pageSize, string? type = null)
    {
        var url = $"api/lookups?pageNumber={pageNumber}&pageSize={pageSize}";
        if (!string.IsNullOrEmpty(type))
        {
            url += $"&type={Uri.EscapeDataString(type)}";
        }

        return await _httpClient.GetFromJsonAsync<PaginatedList<LookupDto>>(url)
               ?? new PaginatedList<LookupDto>(new List<LookupDto>(), 0, pageNumber, pageSize);
    }

    public async Task<List<LookupDto>> GetLookupsByTypeAsync(string type)
    {
        return await _httpClient.GetFromJsonAsync<List<LookupDto>>($"api/lookups/bytype?type={Uri.EscapeDataString(type)}")
               ?? new List<LookupDto>();
    }

    public async Task<List<LookupDto>> GetPublicLookupsByTypeAsync(string type)
    {
        // Uses the public endpoint that only requires authentication (no lookups.read permission)
        return await _httpClient.GetFromJsonAsync<List<LookupDto>>($"api/lookups/public/bytype?type={Uri.EscapeDataString(type)}")
               ?? new List<LookupDto>();
    }

    public async Task<LookupDto?> GetLookupByIdAsync(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<LookupDto>($"api/lookups/{id}");
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<LookupDto> CreateLookupAsync(CreateLookupRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/lookups", request);
        await response.HandleErrorAsync();
        return await response.Content.ReadFromJsonAsync<LookupDto>() ?? throw new Exception("Failed to create lookup");
    }

    public async Task<LookupDto> UpdateLookupAsync(Guid id, UpdateLookupRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/lookups/{id}", request);
        await response.HandleErrorAsync();
        return await response.Content.ReadFromJsonAsync<LookupDto>() ?? throw new Exception("Failed to update lookup");
    }

    public async Task DeleteLookupAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/lookups/{id}");
        await response.HandleErrorAsync();
    }

    public async Task<(bool CodeExists, bool NameEnExists, bool NameArExists)> CheckLookupExistsAsync(string code, string nameEn, string nameAr, Guid? excludeLookupId = null)
    {
        try
        {
            var url = $"api/lookups/exists?code={Uri.EscapeDataString(code)}&nameEn={Uri.EscapeDataString(nameEn)}&nameAr={Uri.EscapeDataString(nameAr)}";
            if (excludeLookupId.HasValue)
            {
                url += $"&excludeLookupId={excludeLookupId.Value}";
            }
            var response = await _httpClient.GetFromJsonAsync<JsonElement>(url);
            return (
                response.GetProperty("codeExists").GetBoolean(),
                response.GetProperty("nameEnExists").GetBoolean(),
                response.GetProperty("nameArExists").GetBoolean()
            );
        }
        catch
        {
            return (false, false, false);
        }
    }
}
