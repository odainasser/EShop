namespace Eshop.Domain.Entities.Museum;

/// <summary>
/// هيئة متاحف الشارقة
///Sharjah Museums Department
///ادارة تقنية المعلومات
///Information and Technology Department
///إدارة تطوير المتاحف
///Museums Development Department
/// </summary>
public class Departments : BaseEntity, IId<int>
{
    public Departments()
    {
        Sections = new HashSet<Sections>();
        DepartmentTranslations = new HashSet<DepartmentTranslations>();
    }

    public int Id { get; set; }

    public string Code { get; set; }

    #region Collections

    [TranslationsSet]
    public virtual ICollection<DepartmentTranslations> DepartmentTranslations { get; set; }

    public virtual ICollection<Sections> Sections { get; set; }

    #endregion
}

[TranslationFor(typeof(Departments))]
public class DepartmentTranslations : TranslatedEntity, IId<int>
{
    public int Id { get; set; }
    public int LanguageId { get; set; }

    public string Title { get; set; }
    public string Desc { get; set; }

    #region Relations

    public int DepartmentId { get; set; }
    public virtual Departments Department { get; set; }

    #endregion
}