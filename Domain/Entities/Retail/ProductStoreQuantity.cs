namespace Eshop.Domain.Entities.Retail;

[Table("ProductStoreQuantities", Schema = SchemaConstant.Retail)]
public class ProductStoreQuantity : BaseEntity, IId<int>, ISectionEntity
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    [ForeignKey(nameof(Section))]
    public int SectionId { get; set; }
    
    public virtual Sections Section { get; set; }

    [ForeignKey(nameof(Store))]
    public int StoreId { get; set; }

    public virtual Store Store { get; set; }

    [ForeignKey(nameof(Product))]
    public int ProductId { get; set; }

    public virtual Product Product { get; set; }
}