namespace Eshop.Domain.Entities.Retail;

[Table("Categories", Schema = SchemaConstant.Retail)]
public class Category : BaseEntity, IId<int>
{
    public int Id { get; set; }

    public virtual ICollection<CategoryTranslation> CategoryTranslations { get; set; }
}

[Table(nameof(CategoryTranslation) + "s", Schema = SchemaConstant.Retail)]
public class CategoryTranslation : BaseEntity, IId<int>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }

    public virtual Category Category { get; set; }
    
    [ForeignKey(nameof(Language))]
    public int LanguageId { get; set; }

    public virtual Languages Language { get; set; }
}