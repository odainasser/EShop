using System.Text.Json.Serialization;
using Application.Common.Models;
using Application.Features.Lookups;
using Application.Services;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Features.UserLogs;
using Domain.Enums;
using Application.Common.Interfaces;

namespace Infrastructure.Services;

public class LookupService : ILookupService
{
    private readonly ApplicationDbContext _context;
    private readonly IUserLogService _userLogService;
    private readonly ICurrentUserService _currentUserService;

    public LookupService(
        ApplicationDbContext context,
        IUserLogService userLogService,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _userLogService = userLogService;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedList<LookupDto>> GetAllLookupsAsync(int pageNumber, int pageSize, string? type = null)
    {
        var query = _context.Lookups
            .Include(l => l.Parent)
            .AsQueryable();

        // Filter by type if provided
        if (!string.IsNullOrWhiteSpace(type) && Enum.TryParse<LookupType>(type, ignoreCase: true, out var parsedType))
        {
            query = query.Where(l => l.Type == parsedType);
        }

        var projectedQuery = query
            // newest first
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new LookupDto
            {
                Id = l.Id,
                Code = l.Code,
                NameEn = l.NameEn,
                NameAr = l.NameAr,
                Type = l.Type.ToString(),
                ParentId = l.ParentId,
                ParentName = l.Parent != null ? l.Parent.NameEn : null,
                ParentNameAr = l.Parent != null ? l.Parent.NameAr : null,
                IsActive = l.IsActive
            });

        var count = await projectedQuery.CountAsync();
        var items = await projectedQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedList<LookupDto>(items, count, pageNumber, pageSize);
    }

    public async Task<List<LookupDto>> GetLookupsByTypeAsync(string type)
    {
        if (string.IsNullOrWhiteSpace(type)) return new List<LookupDto>();

        if (!Enum.TryParse<LookupType>(type, ignoreCase: true, out var parsedType))
        {
            return new List<LookupDto>();
        }

        return await _context.Lookups
            .Where(l => l.Type == parsedType)
            .Include(l => l.Parent)
            // newest first
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new LookupDto
            {
                Id = l.Id,
                Code = l.Code,
                NameEn = l.NameEn,
                NameAr = l.NameAr,
                Type = l.Type.ToString(),
                ParentId = l.ParentId,
                ParentName = l.Parent != null ? l.Parent.NameEn : null,
                ParentNameAr = l.Parent != null ? l.Parent.NameAr : null,
                IsActive = l.IsActive
            })
            .ToListAsync();
    }

    public async Task<LookupDto?> GetLookupByIdAsync(Guid id)
    {
        var lookup = await _context.Lookups
            .Include(l => l.Parent)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (lookup == null) return null;

        return new LookupDto
        {
            Id = lookup.Id,
            Code = lookup.Code,
            NameEn = lookup.NameEn,
            NameAr = lookup.NameAr,
            Type = lookup.Type.ToString(),
            ParentId = lookup.ParentId,
            ParentName = lookup.Parent?.NameEn,
            ParentNameAr = lookup.Parent?.NameAr,
            IsActive = lookup.IsActive,
            CreatedAt = lookup.CreatedAt,
            UpdatedAt = lookup.UpdatedAt
        };
    }

    public async Task<LookupDto> CreateLookupAsync(CreateLookupRequest request)
    {
        var lookup = new Lookup
        {
            Id = Guid.NewGuid(),
            Code = request.Code,
            NameEn = request.NameEn,
            NameAr = request.NameAr,
            Type = ParseLookupType(request.Type),
            ParentId = request.ParentId,
            IsActive = request.IsActive
        };

        _context.Lookups.Add(lookup);
        await _context.SaveChangesAsync();

        // Log action
        var (currentUserId, currentUserName) = await _currentUserService.GetCurrentUserAsync();
        if (currentUserId != Guid.Empty)
        {
            await _userLogService.LogAsync(new CreateUserLogRequest
            {
                UserId = currentUserId,
                UserName = currentUserName,
                Action = AuditAction.Created,
                EntityName = "Lookup",
                EntityId = lookup.Id.ToString()
            });
        }

        return await GetLookupByIdAsync(lookup.Id) ?? throw new InvalidOperationException("Failed to create lookup");
    }

    public async Task<LookupDto> UpdateLookupAsync(Guid id, UpdateLookupRequest request)
    {
        var lookup = await _context.Lookups.FindAsync(id);
        if (lookup == null) throw new KeyNotFoundException($"Lookup with ID {id} not found");

        lookup.Code = request.Code;
        lookup.NameEn = request.NameEn;
        lookup.NameAr = request.NameAr;
        lookup.Type = ParseLookupType(request.Type);
        lookup.ParentId = request.ParentId;
        lookup.IsActive = request.IsActive;

        await _context.SaveChangesAsync();

        // Log action
        var (currentUserId, currentUserName) = await _currentUserService.GetCurrentUserAsync();
        if (currentUserId != Guid.Empty)
        {
            await _userLogService.LogAsync(new CreateUserLogRequest
            {
                UserId = currentUserId,
                UserName = currentUserName,
                Action = AuditAction.Updated,
                EntityName = "Lookup",
                EntityId = lookup.Id.ToString()
            });
        }

        return await GetLookupByIdAsync(id) ?? throw new InvalidOperationException("Failed to update lookup");
    }

    public async Task DeleteLookupAsync(Guid id)
    {
        var lookup = await _context.Lookups.FindAsync(id);
        if (lookup != null)
        {
            // Use Remove so ApplicationDbContext will convert it to a soft-delete (sets IsDeleted, DeletedAt, DeletedBy)
            _context.Lookups.Remove(lookup);
            await _context.SaveChangesAsync();

            // Log action
            var (currentUserId, currentUserName) = await _currentUserService.GetCurrentUserAsync();
            if (currentUserId != Guid.Empty)
            {
                await _userLogService.LogAsync(new CreateUserLogRequest
                {
                    UserId = currentUserId,
                    UserName = currentUserName,
                    Action = AuditAction.Deleted,
                    EntityName = "Lookup",
                    EntityId = lookup.Id.ToString()
                });
            }
        }
    }

    private static LookupType ParseLookupType(string? type)
    {
        if (string.IsNullOrWhiteSpace(type)) return LookupType.Unknown;
        if (Enum.TryParse<LookupType>(type, ignoreCase: true, out var parsed)) return parsed;
        return LookupType.Unknown;
    }

    public async Task<(bool CodeExists, bool NameEnExists, bool NameArExists)> CheckLookupExistsAsync(string code, string nameEn, string nameAr, Guid? excludeLookupId = null)
    {
        var query = _context.Lookups.AsQueryable();
        
        if (excludeLookupId.HasValue)
        {
            query = query.Where(l => l.Id != excludeLookupId.Value);
        }

        var codeExists = await query.AnyAsync(l => l.Code == code);
        var nameEnExists = await query.AnyAsync(l => l.NameEn == nameEn);
        var nameArExists = await query.AnyAsync(l => l.NameAr == nameAr);
        
        return (codeExists, nameEnExists, nameArExists);
    }
}
