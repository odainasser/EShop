namespace Eshop.Domain.Entities.Lukups;

public class SiteVisitTickets : BaseEntity, IId<long>
{
    public long Id { get; set; }
    public long SiteVisitId { get; set; }
    public int SectionTicketId { get; set; }
    public int VisitsCount { get; set; }
    public DateTime LastCountVisitDate { get; set; }

    public virtual SiteVisits SiteVisit { get; set; }
    public virtual SectionTickets SectionTicket { get; set; }

}
