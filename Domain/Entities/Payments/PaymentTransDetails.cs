namespace Eshop.Domain.Entities.Payments;

public class PaymentTransDetails : BaseEntity, IId<long>
{
    public long Id { get; set; }

    /// <summary>
    /// tahseel reference number and it's the same as <see cref="PaymentTrans.RefNum"/>
    /// </summary>
    public string PaymentTransRefNum { get; set; }

    /// <summary>
    /// please use <see cref="EnumPaymentTransDetailItemTypes"/>
    /// T || P || G  => Ticket, Product, or Gift
    /// </summary>
    public string ItemType { get; set; } // T || P || G  => Ticket, Product, or Gift

    /// <summary>
    /// is <see cref="OrderTickets"/> id and <see cref="ItemReferenceId"/>
    /// </summary>
    public long ItemReferenceId { get; set; } // T: IssuedTicket OrderTicketsDetails, P:..., G:...

    public decimal? TotalAmount { get; set; }

    public decimal? TotalVat { get; set; }

    public decimal? TotalExtraFees { get; set; }

    #region Relations

    public long PaymentTransId { get; set; }
    public virtual PaymentTrans PaymentTrans { get; set; }

    #endregion
}
