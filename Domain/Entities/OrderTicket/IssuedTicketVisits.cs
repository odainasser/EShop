namespace Eshop.Domain.Entities.OrderTicket;

public class IssuedTicketVisits : BaseEntity, IId<long>, ISectionEntity
{
    public long Id { get; set; }

    public DateTime VisitedAt { get; set; }
    public string? Note { get; set; }

    public string? CreateByStaffId { get; set; }

    #region Relations

    public long IssuedTicketId { get; set; }
    public int SectionId { get; set; }
    public virtual IssuedTickets IssuedTicket { get; set; }
    public virtual Sections Section { get; set; }

    #endregion
}
