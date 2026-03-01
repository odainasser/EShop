namespace Eshop.Domain.Entities.Retail;

[Table(nameof(RetailOrder) + "s", Schema = SchemaConstant.Retail)]
public class RetailOrder : BaseEntity, IId<long>
{
    public long Id { get; set; }

    /// <summary>
    /// total without vat
    /// </summary>
    public decimal SubTotal { get; set; }

    public decimal Vat { get; set; }

    public decimal Total { get; set; }

    #region If Export

    public string? CustomerName { get; set; }
    public string? CustomerMobile { get; set; }
    public string? CustomerEmail { get; set; }

    #endregion

    #region if import

    [ForeignKey(nameof(SupplierId))]
    public virtual Supplier? Supplier { get; set; }

    public int? SupplierId { get; set; }

    #endregion

    public virtual ICollection<RetailOrderDetail> RetailOrderDetails { get; set; }
}

[Table(nameof(RetailOrderDetail) + "s", Schema = SchemaConstant.Retail)]
public class RetailOrderDetail : BaseEntity, IId<long>, ISectionEntity
{
    public long Id { get; set; }

    public int Quantity { get; set; }

    /// <summary>
    /// price for one quantity
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// total without vat
    /// </summary>
    public decimal SubTotal { get; set; }

    public decimal Vat { get; set; }

    public decimal Total { get; set; }

    [ForeignKey(nameof(RetailOrderId))]
    public virtual RetailOrder RetailOrder { get; set; }

    public long RetailOrderId { get; set; }

    [ForeignKey(nameof(StoreId))]
    public virtual Store Store { get; set; }

    public int StoreId { get; set; }

    [ForeignKey(nameof(SectionId))]
    public virtual Sections Section { get; set; }

    public int SectionId { get; set; }
}