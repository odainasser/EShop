namespace Eshop.Domain.Entities.Lukups;

/// <summary>
/// contains all facilities lukup
/// admin can add or edit or remove from them
/// </summary>
[Table(nameof(Facility) + "s", Schema = SchemaConstant.Museum)]
public class Facility : BaseEntity, IId<int>
{
    public int Id { get; set; }

    /// <summary>
    /// default name in case of ar , en not found
    /// </summary>
    public string DefaultName { get; set; }

    /// <summary>
    /// icon
    /// </summary>
    public string ImageUrl { get; set; }

    /// <summary>
    /// contains all facilities names
    /// </summary>
    public virtual ICollection<FacilityTranslation> FacilityTranslations { get; set; }
}

/// <summary>
/// contains all facilities names
/// </summary>
[Table(nameof(FacilityTranslation) + "s", Schema = SchemaConstant.Museum)]
public class FacilityTranslation : BaseEntity, IId<int>
{
    public int Id { get; set; }

    /// <summary>
    /// name in ar or en depends on <see cref="LanguageId"/>
    /// </summary>
    public string Name { get; set; }

    [ForeignKey(nameof(Language))]
    public int LanguageId { get; set; }

    public virtual Languages Language { get; set; }

    [ForeignKey(nameof(Facility))]
    public int FacilityId { get; set; }

    public virtual Facility Facility { get; set; }
}
