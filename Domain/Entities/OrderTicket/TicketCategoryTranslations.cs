namespace Eshop.Domain.Entities.OrderTicket;

[TranslationFor(typeof(TicketCategories))]
public class TicketCategoryTranslations : TranslatedEntity, IId<int>
{
    public int Id { get; set; }
    public int LanguageId { get; set; }

    public string Name { get; set; }
    public string Desc { get; set; }

    #region Relations

    public int TicketCategoryId { get; set; }
    public virtual TicketCategories TicketCategory { get; set; }

    #endregion
}
