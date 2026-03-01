namespace Eshop.Domain.Entities.Museum;

/// <summary>
/// it should contains wifi passwords to link them with tickets
/// but now not working
/// </summary>
[AuditableEntity]
public class WifiPassword : BaseEntity, IId<int>, ISectionEntity
{
    // refer to the free tickets issued by the system to marketing team.
    public int Id { get; set; }

    /// <summary>
    /// Museum <see cref="Sections"/>
    /// </summary>
    public int SectionId { get; set; }

    public string Password { get; set; }

    /// <summary>
    /// ends
    /// </summary>
    public bool HasExpiration { get; set; } = false;

    public DateTime? ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public int? UsedBy { get; set; }

    public DateTime? UsedAt { get; set; }

}
