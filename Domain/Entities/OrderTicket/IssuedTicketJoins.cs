namespace Eshop.Domain.Entities.OrderTicket;

public class IssuedTicketJoins : BaseEntity, IId<long>, ISectionEntity
{
    public long Id { get; set; }
    public int? GroupCount { get; set; }
    public int? BabyCount { get; set; }
    public int? ChildCount { get; set; }
    public int? AdultCount { get; set; }

    public int? SeniorsCount { get; set; }
    public int? CompanionsCount { get; set; }
    public int? DisabilitiesCount { get; set; }

    /// <summary>
    /// <see cref="EnumTicketStatus"/>
    /// </summary>
    public string StatusCode { get; set; }
    public DateTime? UsedAt { get; set; }
    public int? ScannedBy { get; set; }
    public string? ScannedByStaffId { get; set; }
    public string? Note { get; set; }
    public string? CreateByStaffId { get; set; }

    #region Relations

    public long IssuedTicketId { get; set; }
    public int SectionId { get; set; }
    public virtual IssuedTickets IssuedTicket { get; set; }
    public virtual Sections Section { get; set; }

    #endregion
}
