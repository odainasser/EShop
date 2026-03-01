namespace Eshop.Domain.Entities.Lukups;

[Table("Countries", Schema = SchemaConstant.Lukups)]

public class Country : BaseEntity, IId<int>
{
    public int Id { get ; set ; }
    public string KeyCode { get ; set ; }
    public string PhoneCode { get ; set ; }
    public string Flag { get ; set ; }
    public virtual ICollection<CountriesTranslation> CountryTranslations { get; set; }

}
[Table(nameof(CountriesTranslation) + "s", Schema = SchemaConstant.Lukups)]
public class CountriesTranslation : BaseEntity, IId<int>
{
    public int Id { get; set; }

    /// <summary>
    /// name in ar or en depends on <see cref="LanguageId"/>
    /// </summary>
    public string Name { get; set; }

    [ForeignKey(nameof(Language))]
    public int LanguageId { get; set; }

    public virtual Languages Language { get; set; }

    [ForeignKey(nameof(Country))]
    public int CountryId { get; set; }

    public virtual Country Country { get; set; }
}