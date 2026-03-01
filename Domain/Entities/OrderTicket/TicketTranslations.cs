namespace Eshop.Domain.Entities.OrderTicket;

[TranslationFor(typeof(Tickets)), AuditableEntity]
public class TicketTranslations : TranslatedEntity, IId<int>
{
    public int Id { get; set; }
    public int LanguageId { get; set; }

    public string Name { get; set; }
    public string Desc { get; set; }

    #region Relations

    public int TicketId { get; set; }
    public virtual Tickets Ticket { get; set; }

    #endregion
}
