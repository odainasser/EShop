namespace Eshop.Domain.Entities.Museum;

/// <summary>
/// ايام العمل بكل متاحف
/// </summary>
[AuditableEntity]
public class SectionsWorkingDays : BaseEntity, IId<int>, ISectionEntity
{
    public int Id { get; set; }

    public string DayCode { get; set; }

    public bool IsOffDay { get; set; } = false;

    public TimeSpan? From { get; set; }

    public TimeSpan? To { get; set; }

    #region Relation

    public int SectionId { get; set; }

    public virtual Sections Section { get; set; }

    #endregion
}
