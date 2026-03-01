namespace Eshop.Domain.Entities.Retail;

[Table(nameof(Supplier) + "s", Schema = SchemaConstant.Retail)]
public class Supplier : BaseEntity, IId<int>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? Mobile { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<RetailOrder> RetailOrders { get; set; }
}