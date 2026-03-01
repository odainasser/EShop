namespace Eshop.Domain.Entities.Payments;

public class TahseelPayTrans : BaseEntity, IId<long>
{
    public TahseelPayTrans()
    {
        TahseelPayTransLogs = new HashSet<TahseelPayTransLogs>();
    }

    public long Id { get; set; }
    public int? UserId { get; set; }
    public string TP_RefNo { get; set; }
    public string Tahseel_POS_TransId { get; set; }
    public int TP_ResultCode { get; set; }
    public string? TP_ReceiptNo { get; set; }
    public DateTime? TP_PaymentDate { get; set; }
    public decimal? TotalAmount { get; set; }
    public int? TP_PayMethod { get; set; }
    public string? TP_ExtraFees { get; set; }
    public string? TP_TaxFees { get; set; }
    public string? TP_SecHash { get; set; }
    public bool IsSuccess { get; set; } = false;
    public string? CancelRequestNumber { get; set; }
    public DateTime? CanceledAt { get; set; }

    #region One to one

    public long PaymentTransId { get; set; }
    /// <summary>
    /// one to one relations ship
    /// <see cref="TahseelPayTrans"/> with <see cref="PaymentTrans"/> by foreign key => 
    /// <see cref="PaymentTransId"/>
    /// </summary>
    public virtual PaymentTrans PaymentTrans { get; set; }

    #endregion

    #region Collections

    public virtual ICollection<TahseelPayTransLogs> TahseelPayTransLogs { get; set; }

    #endregion
}
