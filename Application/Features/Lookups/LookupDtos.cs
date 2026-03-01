using Domain.Enums;

namespace Application.Features.Lookups;

public class LookupDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // Keep string for API compatibility
    public Guid? ParentId { get; set; }
    public string? ParentName { get; set; }
    public string? ParentNameAr { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateLookupRequest
{
    public string Code { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // accept string and parse server-side
    public Guid? ParentId { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateLookupRequest
{
    public string Code { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // accept string and parse server-side
    public Guid? ParentId { get; set; }
    public bool IsActive { get; set; }
}
