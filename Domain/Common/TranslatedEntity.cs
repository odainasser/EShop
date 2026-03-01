namespace Eshop.Domain.Common;

public class TranslatedEntity : BaseEntity
{
    public virtual Languages Language { get; set; }

    public string LangCode => Language?.IsoCode ?? DomainHelpers.GetTransCultureName();
}