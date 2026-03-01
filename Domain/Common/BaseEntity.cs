using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Common;

public abstract class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    [JsonIgnore]
    public bool IsSynced { get; set; }
}

public interface IId<T>
{
    [Key]
    public T Id { get; set; }
}

public interface IBaseEntity
{
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    [JsonIgnore]
    public bool IsSynced { get; set; }
}

public interface ISectionEntity
{
    public int SectionId { get; set; }
}
