namespace Eshop.Domain.Entities.OrderTicket;

/// <summary>
/// كل تذاكر المتاحف جميعا
/// </summary>
[AuditableEntity]
public class Tickets : BaseEntity, IId<int>
{
    public Tickets()
    {
        TicketTranslations = new HashSet<TicketTranslations>();
        SectionTickets = new HashSet<SectionTickets>();
    }

    public int Id { get; set; }
    public string? Code { get; set; }
    //public string POSCode { get; set; }
    public string? TahseelCode { get; set; }
    public string? POSTahseelCode { get; set; }

    /// <summary>
    /// <see cref="EnumTicketBaseTypes"/>
    /// </summary>
    public string? TicketType { get; set; } // {basic  - groupon, bustour, sharjah experience ...}
    public bool ForSenior { get; set; } = false; // for 60+ years
    public bool WithCompanion { get; set; } = false; // morafeq
    public bool ForDisabilities { get; set; } = false;
    public bool IsJoinedSections { get; set; } = false;
    public bool IsGroup { get; set; } = false;
    //public string GroupType { get; set; } this should replaced by AgeGroup
    public string AgeGroup { get; set; }
    public int? MinGroupCount { get; set; } = 0;
    public int? MaxGroupCount { get; set; } = 0;
    public string? ImageUrl { get; set; }
    public decimal? Discount { get; set; }
    public bool IsExternal { get; set; } = false;
    public bool ForOnline { get; set; } = false;
    public bool ForPOS { get; set; } = false;
    public bool IsPromoted { get; set; } = false; // refer to the free tickets issued by the system to marketing team.
    public bool IsForStaff { get; set; } = false; // ticket that will used for only staff
    public bool IsManyVisits { get; set; } = false; // ticket that will used for multiple visits, like basic promotion [buy for three visits and get one visit free]
    public int? VisitsCount { get; set; }

    public bool SameDayUse { get; set; } // mean if ticket is used today, and it has period day limit more than one day, it cannot be used again after today.
    public bool HasPeriodLimit { get; set; } = false;
    public int PeriodDaysLimit { get; set; }

    #region Relations

    public int TicketCategoryId { get; set; }
    public virtual TicketCategories TicketCategory { get; set; }

    #endregion

    #region Collections

    public virtual ICollection<SectionTickets> SectionTickets { get; set; }

    [TranslationsSet]
    public virtual ICollection<TicketTranslations> TicketTranslations { get; set; }

    #endregion
}
