namespace Eshop.Domain.Entities.People;

[TranslationFor(typeof(RoleToPermissions))]
public class RoleToPermissionsTranslations : TranslatedEntity, IId<int>
{
    public int Id { get; set; }
    public int LanguageId { get; set; }

    public string Title { get; set; }
    public string? Desc { get; set; }

    #region Relation

    public int RoleToPermissionsId { get; set; }
    public virtual RoleToPermissions RoleToPermissions { get; set; }

    #endregion
}
