namespace Eshop.Domain.Entities.OrderTicket;

/// <summary>
/// Name	Desc مواطن	مواطن - عربى مقيم	عربى مقيم
/// </summary>
[AuditableEntity]
public class TicketCategories : BaseEntity, IId<int>
{
    public TicketCategories()
    {
        Tickets = new HashSet<Tickets>();
        TicketCategoryTranslations = new HashSet<TicketCategoryTranslations>();
    }

    public int Id { get; set; }
    public string Code { get; set; }
    public string? POSCode { get; set; }
    public string TahseelCode { get; set; }
    public string POSTahseelCode { get; set; }
    public string? ImgSrc { get; set; }
    public int SortNo { get; set; }

    #region Collections

    [TranslationsSet]
    public virtual ICollection<TicketCategoryTranslations> TicketCategoryTranslations { get; set; }

    public virtual ICollection<Tickets> Tickets { get; set; }

    #endregion
}
