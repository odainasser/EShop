namespace Eshop.Domain.Entities.Retail;

[Table(nameof(Store) + "s", Schema = SchemaConstant.Retail)]
public class Store : BaseEntity, IId<int>, ISectionEntity
{
    public int Id { get; set; }

    [ForeignKey(nameof(Section))]
    public int SectionId { get; set; }

    public virtual Sections Section { get; set; }

    public virtual ICollection<StoreTranslation> StoreTranslations { get; set; }

    public virtual ICollection<RetailOrderDetail> RetailOrderDetails { get; set; }
}

[Table(nameof(StoreTranslation) + "s", Schema = SchemaConstant.Retail)]
public class StoreTranslation : BaseEntity, IId<int>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    [ForeignKey(nameof(Store))]
    public int StoreId { get; set; }

    public virtual Store Store { get; set; }

    [ForeignKey(nameof(Language))]
    public int LanguageId { get; set; }

    public virtual Languages Language { get; set; }
}