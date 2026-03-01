using Newtonsoft.Json;

namespace Eshop.Domain.Common;

public abstract class BaseEntity : IBaseEntity
{
    public bool IsActive { get; set; } = true;
    public int? CreateBy { get; set; }
    public DateTime CreateAt { get; set; }
    public int? EditBy { get; set; }
    public DateTime? EditAt { get; set; }
    public int? DeletedBy { get; set; }

    /// <summary>
    /// if this contains value then item is deleted
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    [JsonIgnore]
    public bool IsSynced { get; set; }
}

public interface IId<T>
{
    /// <summary>
    /// Primary key
    /// </summary>
    [Key]
    public T Id { get; set; }
}

public interface IBaseEntity
{
    public bool IsActive { get; set; }
    public int? CreateBy { get; set; }
    public DateTime CreateAt { get; set; }
    public int? EditBy { get; set; }
    public DateTime? EditAt { get; set; }
    public int? DeletedBy { get; set; }

    /// <summary>
    /// if this contains value then item is deleted
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    [JsonIgnore]
    public bool IsSynced { get; set; }
}

public interface ISectionEntity
{
    public int SectionId { get; set; }
}