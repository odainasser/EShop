using Application.Common.Models;
using Application.Features.Lookups;

namespace Application.Services;

public interface ILookupService
{
    Task<PaginatedList<LookupDto>> GetAllLookupsAsync(int pageNumber, int pageSize, string? type = null);
    Task<List<LookupDto>> GetLookupsByTypeAsync(string type);
    Task<LookupDto?> GetLookupByIdAsync(Guid id);
    Task<LookupDto> CreateLookupAsync(CreateLookupRequest request);
    Task<LookupDto> UpdateLookupAsync(Guid id, UpdateLookupRequest request);
    Task DeleteLookupAsync(Guid id);
    Task<(bool CodeExists, bool NameEnExists, bool NameArExists)> CheckLookupExistsAsync(string code, string nameEn, string nameAr, Guid? excludeLookupId = null);
}
