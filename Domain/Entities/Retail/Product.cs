namespace Eshop.Domain.Entities.Retail;

[Table(nameof(Product) + "s", Schema = SchemaConstant.Retail)]
public class Product : BaseEntity, IId<int>
{
    public int Id { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<ProductTranslation> ProductTranslations { get; set; }
}

[Table(nameof(ProductTranslation) + "s", Schema = SchemaConstant.Retail)]
public class ProductTranslation : BaseEntity, IId<int>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    [ForeignKey(nameof(Product))]
    public int ProductId { get; set; }

    public virtual Product Product { get; set; }

    [ForeignKey(nameof(Language))]
    public int LanguageId { get; set; }

    public virtual Languages Language { get; set; }
}