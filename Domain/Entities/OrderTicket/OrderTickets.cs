namespace Eshop.Domain.Entities.OrderTicket;

/// <summary>
/// Contains order tickets 
/// you can access <see cref="SectionTickets"/> and from it you can access <see cref="Ticket"/>
/// so you can get Ticket Name for adult or what ,.....
/// </summary>
public class OrderTickets : BaseEntity, IId<long>
{
    public long Id { get; set; }

    public string ItemTahseelCode { get; set; }
    public bool IsReservation { get; set; } = false;

    /// <summary>
    /// <see cref="EnumOrderTicketPaymentStatus"/>
    /// </summary>
    public int PaymentStatus { get; set; } // Free | wait Pay | Payment Done
    public bool IsJoinedSections { get; set; } = false;
    public bool IsGroup { get; set; } = false;
    public string AgeGroup { get; set; }
    public int MinGroupCount { get; set; } = 0;
    public int MaxGroupCount { get; set; } = 0;
    public int? GroupCount { get; set; }
    public int BabyCount { get; set; } = 0;
    public int ChildCount { get; set; } = 0;
    public int AdultCount { get; set; } = 0;
    public DateTime? AttendanceDate { get; set; }

    /// <summary>
    /// Price for one Ticket
    /// </summary>
    public decimal UnitPrice { get; set; } = 0;
    public decimal UnitVat { get; set; } = 0;
    public int Qty { get; set; } = 0;
    public decimal SubTotal { get; set; } = 0;
    public decimal TotalVat { get; set; } = 0;
    public decimal Discount { get; set; } = 0;

    public EnumVatPayer VatPayer { get; set; } = EnumVatPayer.SMA;

    public decimal NetPrice => VatPayer == EnumVatPayer.SMA
        ? UnitPrice + UnitVat - Discount
        : UnitPrice - Discount;

    public decimal? GrantTotal => SubTotal + TotalVat - Discount;

    public bool IsFree { get; set; } = false;

    #region Relations

    public long? OrderId { get; set; }

    public int SectionTicketId { get; set; }
    public virtual Orders Order { get; set; }
    public virtual SectionTickets SectionTicket { get; set; }

    #endregion
}