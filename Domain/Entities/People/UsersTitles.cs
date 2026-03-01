namespace Eshop.Domain.Entities.People;

public class UsersTitles : BaseEntity, IId<int>
{
    public UsersTitles()
    {
        Users = new HashSet<ApplicationUser>();
        UsersTitleTranslations = new HashSet<UsersTitleTranslations>();
    }

    public int Id { get; set; }
    public string Code { get; set; }

    #region Collections

    [TranslationsSet]
    public virtual ICollection<UsersTitleTranslations> UsersTitleTranslations { get; set; }

    public virtual ICollection<ApplicationUser> Users { get; set; }

    #endregion
}

[TranslationFor(typeof(UsersTitles))]
public class UsersTitleTranslations : TranslatedEntity, IId<int>
{
    public int Id { get; set; }
    public int LanguageId { get; set; }
    public string Title { get; set; }
    public string? Desc { get; set; }

    #region Relations

    public int UsersTitleId { get; set; }
    public virtual UsersTitles UsersTitle { get; set; }

    #endregion
}
