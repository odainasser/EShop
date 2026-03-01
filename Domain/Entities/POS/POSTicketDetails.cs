// ReSharper disable InconsistentNaming
namespace Eshop.Domain.Entities.POS;

public class POSTicketDetails : BaseEntity, IId<int>
{
    public int Id { get; set; }

    public string ItemPackageId { get; set; }
    public int IBookMark { get; set; }
    public decimal? Price { get; set; }
    public int Qty { get; set; }
    public decimal? DiscountValue { get; set; }
    public decimal? DiscountRate { get; set; }

    public decimal? TaxValue { get; set; }
    public decimal? TaxRate { get; set; }

    public decimal? FinalPrice { get; set; }
    public decimal? ItemNetVal { get; set; }

    #region Relations

    public long POSTicketId { get; set; }
    public int? SectionTicketId { get; set; } = null;
    public virtual POSTickets POSTicket { get; set; }
    public virtual SectionTickets SectionTicket { get; set; }

    #endregion
    //public virtual POSLocTickets POSLocTicket { get; set; }
    
}
