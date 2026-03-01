namespace Eshop.Domain.Entities.Lukups;

public class SiteVisits : BaseEntity,IId<long>
{
    public SiteVisits() => SiteVisitTickets = new HashSet<SiteVisitTickets>();

    public long Id { get; set; }
    public string VisitGUID { get; set; }

    public bool? IsAdmin { get; set; }
    public string VisitSource { get; set; }
    public string? UserAgent { get; set; }
    public string IP { get; set; }
    public string Lang { get; set; }
    public int VisitsCount { get; set; }
    public DateTime VisitDate { get; set; }
    public DateTime LastCountVisitDate { get; set; }
    public DateTime LastSessionCompareDate { get; set; }
    public DateTime LastVisitDatePerDay { get; set; }
    public int VisitSessionMinutes { get; set; }
    public int TotalVisitSessionMinutes { get; set; }

    #region Relations

    public int? UserId { get; set; }
    public virtual ApplicationUser User { get; set; }

    #endregion

    #region Collections

    public virtual ICollection<SiteVisitTickets> SiteVisitTickets { get; set; }

    #endregion

}
