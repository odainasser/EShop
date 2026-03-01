namespace Eshop.Domain.Entities.OrderTicket;

public class OrderProducts : BaseEntity, IId<long>
{
    public long Id { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? UnitVat { get; set; }
    public int? Qty { get; set; }
    public decimal? TotalAmount { get; set; }
    public decimal? TotalVat { get; set; }
    public decimal? Discount { get; set; }
    public bool IsFree { get; set; } = false;

    #region Relations

    public long? OrderId { get; set; }
    public virtual Orders Order { get; set; }

    #endregion
}
