// ReSharper disable InconsistentNaming
namespace Eshop.Domain.Entities.POS;

public class POSTickets : BaseEntity, IId<long>, ISectionEntity
{
    public POSTickets()
    {
        //POSTicketSections = new HashSet<POSTicketSections>();
        //POSScanTicketLogs = new HashSet<POSScanTicketLogs>();
        POSTicketDetails = new HashSet<POSTicketDetails>();
    }

    public long Id { get; set; }

    public string? SectionTicketCode { get; set; }

    public int HeaderId { get; set; }
    public int SalesNo { get; set; }
    public int POSNo { get; set; }
    public string StoreId { get; set; }
    public string POSTicketType { get; set; }
    public string MuseumCode { get; set; }


    public string TransDate { get; set; }
    public string TransTime { get; set; }

    public string TicketType { get; set; }
    public string TicketRefNumber { get; set; }
    public bool IsExternal { get; set; } = false;
    public string? ExternalRefNumber { get; set; }

    public DateTime? AttendanceDate { get; set; }
    public DateTime? IssueDate { get; set; }

    public int UserId { get; set; }
    public string UserStaffId { get; set; }

    public decimal? DiscountValue { get; set; }
    public decimal? DiscountRate { get; set; }
    public decimal? ResearchFees { get; set; }

    public string TransValue { get; set; }
    public decimal? NetTransValue { get; set; }

    public decimal? PaidValue { get; set; }
    public decimal? ReturnsValue { get; set; }

    public decimal? ServiceValue { get; set; }
    public decimal? ServiceRate { get; set; }

    public decimal? PlusDiscountRate { get; set; }

    /// <summary>
    /// <see cref="EnumPaymentTypes"/>
    /// </summary>
    public string? PaymentMethod { get; set; }
    public bool IsByVisa { get; set; } = false;
    public string? TP_ExtraFees { get; set; }
    public string? TP_TaxFees { get; set; }
    public string Notes { get; set; }

    public bool NeedForceDelete { get; set; } = false;
    public bool WaitPayment { get; set; } = false;
    public bool IsHoldList { get; set; } = false;

    #region Relations

    public int SectionId { get; set; }
    public virtual Sections Section { get; set; }
    public int SectionTicketId { get; set; }
    public virtual SectionTickets SectionTicket { get; set; }

    #endregion

    #region Collections

    public virtual ICollection<POSTicketDetails> POSTicketDetails { get; set; }

    #endregion
}
