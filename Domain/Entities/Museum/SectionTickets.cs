namespace Eshop.Domain.Entities.Museum;

/// <summary>
/// تحتوي علي جميع التذاكر لكل متحف بالسعر والتفاصيل 
/// تذاكر المتحف
/// </summary>
[AuditableEntity]
public class SectionTickets : BaseEntity, IId<int>, ISectionEntity
{
    public SectionTickets()
    {
        CartTickets = new HashSet<CartTickets>();
        OrderTickets = new HashSet<OrderTickets>();
        SectionTicketJoins = new HashSet<SectionTicketJoins>();
        IssuedTickets = new HashSet<IssuedTickets>();
        PromotedTickets = new HashSet<PromotedTicket>();
        POSTickets = new HashSet<POSTickets>();
        POSTicketDetails = new HashSet<POSTicketDetails>();
        IssuedTicketScanLogs = new HashSet<IssuedTicketScanLogs>();
        SiteVisitTickets = new HashSet<SiteVisitTickets>();
    }

    public int Id { get; set; }

    public string? POSCode { get; set; }

    /// <summary>
    /// if you want to set ticket free for a museum
    /// set this to true
    /// and if you want to set it back to price
    /// set this to false if price is zero
    /// </summary>
    public bool IsFree { get; set; } = false;
    public decimal Price { get; set; } = 0;
    public decimal Vat { get; set; } = 0;
    public decimal Discount { get; set; } = 0;

    /// <summary>
    /// to determine=> vat will be sent within tahseel request 
    /// </summary>
    public EnumVatPayer VatPayer { get; set; } = EnumVatPayer.SMA;

    public bool HasGlobalDiscount { get; set; } = false;

    /// <summary>
    /// <see cref="EnumTicketUsage"/>
    /// </summary>
    public bool ForOnline { get; set; } = false;

    /// <summary>
    /// <see cref="EnumTicketUsage"/>
    /// </summary>
    public bool ForPOS { get; set; } = false;

    public int sortNo { get; set; }

    public bool IsSynced { get; set; }

    #region Relations

    public int TicketId { get; set; }
    public int SectionId { get; set; }
    public virtual Tickets Ticket { get; set; }
    public virtual Sections Section { get; set; }

    #endregion

    #region Collections

    public virtual ICollection<CartTickets> CartTickets { get; set; }
    public virtual ICollection<OrderTickets> OrderTickets { get; set; }
    public virtual ICollection<SectionTicketJoins> SectionTicketJoins { get; set; }
    public virtual ICollection<IssuedTickets> IssuedTickets { get; set; }
    public virtual ICollection<PromotedTicket> PromotedTickets { get; set; }
    public virtual ICollection<POSTickets> POSTickets { get; set; }
    public virtual ICollection<POSTicketDetails> POSTicketDetails { get; set; }
    public virtual ICollection<IssuedTicketScanLogs> IssuedTicketScanLogs { get; set; }
    public virtual ICollection<SiteVisitTickets> SiteVisitTickets { get; set; }

    #endregion
}
