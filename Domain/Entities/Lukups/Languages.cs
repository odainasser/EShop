namespace Eshop.Domain.Entities.Lukups;

public class Languages : BaseEntity, IId<int>
{
    public Languages()
    {
        TicketCategoryTranslations = new HashSet<TicketCategoryTranslations>();
        TicketTranslations = new HashSet<TicketTranslations>();
        DepartmentTranslations = new HashSet<DepartmentTranslations>();
        SectionTranslations = new HashSet<SectionTranslations>();
        RoleToPermissionsTranslations = new HashSet<RoleToPermissionsTranslations>();
        UsersTitleTranslations = new HashSet<UsersTitleTranslations>();
    }

    public int Id { get; set; }

    /// <summary>
    /// Name of the language
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// code ar,en
    /// </summary>
    public string IsoCode { get; set; }

    #region Collections

    public virtual ICollection<TicketCategoryTranslations> TicketCategoryTranslations { get; set; }
    public virtual ICollection<TicketTranslations> TicketTranslations { get; set; }
    public virtual ICollection<DepartmentTranslations> DepartmentTranslations { get; set; }
    public virtual ICollection<SectionTranslations> SectionTranslations { get; set; }
    public virtual ICollection<RoleToPermissionsTranslations> RoleToPermissionsTranslations { get; set; }
    public virtual ICollection<UsersTitleTranslations> UsersTitleTranslations { get; set; }

    #endregion
}
