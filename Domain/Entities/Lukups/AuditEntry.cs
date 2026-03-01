namespace Eshop.Domain.Entities.Lukups;

[NotMapped]
public class AuditEntry(EntityEntry entry)
{
    public EntityEntry Entry { get; } = entry;
    public string UserId { get; set; }
    public string TableName { get; set; }
    public Dictionary<string, object> KeyValues { get; } = new();
    public Dictionary<string, object> ForeignkeyValues { get; } = new();
    public Dictionary<string, object> OldValues { get; } = new();
    public Dictionary<string, object> NewValues { get; } = new();
    public EnumAuditType AuditType { get; set; }
    public List<string> ChangedColumns { get; } = new();
}