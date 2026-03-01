namespace Eshop.Domain.Entities.Payments;

public class PaymentTrans : BaseEntity, IId<long>
{
    public PaymentTrans()
    {
        PaymentTransDetails = new HashSet<PaymentTransDetails>();
        TahseelPayTransLogs = new HashSet<TahseelPayTransLogs>();
    }

    public long Id { get; set; }

    /// <summary>
    /// reference number auti generate by database from sequence shared.PaymentTransNumbers
    /// </summary>
    public long PaySeqRef { get; set; }

    /// <summary>
    /// Tahseel reference number start with PAY
    /// </summary>
    public string RefNum { get; set; }

    /// <summary>
    ///is <see cref="Orders.OrderRef"/>
    /// </summary>
    public string OrderRefNum { get; set; }

    public int? UserId { get; set; }
    public int ChannelCode { get; set; }
    public string? SectionCode { get; set; }
    public string? POSCode { get; set; }

    public decimal? TotalAmount { get; set; }
    public decimal? TotalVat { get; set; }
    public decimal? TotalExtraFees { get; set; }

    /// <summary>
    /// <see cref="EnumPaymentTransStatus"/>
    /// </summary>
    public int StatusCode { get; set; }
    public bool HasTPError { get; set; } = false;
    public string? ErrorMessage { get; set; }
    public string? TP_SecHash { get; set; }

    public bool isChecked { get; set; } = false;
    public DateTime? CheckedAt { get; set; }

    #region Relations

    /// <summary>
    /// one to one relations ship
    /// <see cref="TahseelPayTrans"/> with <see cref="PaymentTrans"/> by foreign key => PaymentTransId
    /// </summary>
    public virtual TahseelPayTrans TahseelPayTrans { get; set; }

    public long OrderId { get; set; }
    public virtual Orders Order { get; set; }

    #endregion

    #region Collections

    public virtual ICollection<TahseelPayTransLogs> TahseelPayTransLogs { get; set; }

    public virtual ICollection<PaymentTransDetails> PaymentTransDetails { get; set; }

    #endregion
}
