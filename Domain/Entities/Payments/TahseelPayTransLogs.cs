namespace Eshop.Domain.Entities.Payments;

public class TahseelPayTransLogs : BaseEntity, IId<long>
{
    public long Id { get; set; }

    public int? UserId { get; set; }

    public string TP_RefNo { get; set; }
    public int TP_ResultCode { get; set; }
    public string? TP_ReceiptNo { get; set; }
    public DateTime? TP_PaymentDate { get; set; }
    public decimal? TotalAmount { get; set; }
    public int? TP_PayMethod { get; set; }
    public string? TP_ExtraFees { get; set; }
    public string? TP_TaxFees { get; set; }
    public string? TP_SecHash { get; set; }
    public bool IsSuccess { get; set; }
    public DateTime? TransCreateAt { get; set; }
    

    #region Relations

    public long? TahseelPayTransId { get; set; }
    public virtual TahseelPayTrans TahseelPayTrans { get; set; }

    public long PaymentTransId { get; set; }
    public virtual PaymentTrans PaymentTrans { get; set; }

    #endregion
}
