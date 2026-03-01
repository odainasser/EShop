namespace Eshop.Domain.Entities.OrderTicket;

/// <summary>
/// may be online may be from staff
/// </summary>
public class Orders : BaseEntity, IId<long>
{
    public Orders()
    {
        OrderProducts = new HashSet<OrderProducts>();
        OrderTickets = new HashSet<OrderTickets>();
        PaymentTrans = new HashSet<PaymentTrans>();
    }

    public long Id { get; set; }
    /// <summary>
    /// 100005821
    /// </summary>
    public long OrderSeqRef { get; set; }
    public decimal Vat { get; set; } = 0;
    /// <summary>
    /// ORD151698002806
    /// </summary>
    public string OrderRef { get; set; }
    public string MainDepTahseelCode { get; set; }

    public string FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public string? Email { get; set; }

    /// <summary>
    /// user address 
    /// currently empty
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// <see cref="EnumOrderChannels"/> order made from which source: website | POS ...
    /// </summary>
    public int ChannelCode { get; set; } // order made from which source: website | POS ...
    public string? POSCode { get; set; }

    /// <summary>
    /// <see cref="EnumOrderStatus"/>
    /// </summary>
    public string StatusCode { get; set; }
    public decimal SubTotal { get; set; } = 0;
    public decimal TotalAmount { get; set; } = 0;
    public decimal PaidValue { get; set; } = 0;
    public decimal ReturnsValue { get; set; } = 0;

    /// <summary>
    /// ResearchFees not vat again not vat
    /// if SubTotal larger than or equals 50 AED_then this will be 10 AED
    /// </summary>
    public decimal ExtraFees { get; set; } = 0;

    /// <summary>
    /// <see cref="EnumPaymentTypes"/>
    /// </summary>
    public string PaymentTypeCode { get; set; }
    public bool ForGuest { get; set; } = false;
    public bool HasPaidItems { get; set; } = false;

    /// <summary>
    /// is all tickets paid it worked
    /// </summary>
    public bool IsFullPaid { get; set; } = false;
    public string? TP_SecHash { get; set; }
   
    /// <summary>
    /// i think it related to cach 
    /// </summary>
    public bool IsPosted { get; set; } = false;

    /// <summary>
    /// in pos ticket only
    /// </summary>
    public string? WifiCode { get; set; }

    /// <summary>
    /// External booking number (from third-party booking system)
    /// </summary>
    public string? BookingNumber { get; set; }

    #region Relations

    /// <summary>
    /// User Id
    /// </summary>
    public int? UserId { get; set; }

    /// <summary>
    /// Museum
    /// </summary>
    public int? SectionId { get; set; }

    /// <summary>
    /// <see cref="UserId"/>
    /// </summary>
    public virtual ApplicationUser User { get; set; }

    /// <summary>
    /// <see cref="SectionId"/>
    /// </summary>
    public virtual Sections Section { get; set; }

    #endregion

    #region Collections

    public virtual ICollection<OrderProducts> OrderProducts { get; set; }
    public virtual ICollection<OrderTickets> OrderTickets { get; set; }
    public virtual ICollection<PaymentTrans> PaymentTrans { get; set; }


    #endregion
}
