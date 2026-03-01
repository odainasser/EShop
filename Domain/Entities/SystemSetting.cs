using Domain.Common;

namespace Domain.Entities;

public class SystemSetting : BaseAuditableEntity
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Group { get; set; } = string.Empty; // e.g., "Email", "General"
}
