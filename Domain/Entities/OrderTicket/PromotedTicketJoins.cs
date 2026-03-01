namespace Eshop.Domain.Entities.OrderTicket;

public class PromotedTicketJoins : BaseEntity, IId<int>, ISectionEntity
{
    public int Id { get; set; }

    #region Relations

    public int PromotedTicketId { get; set; }
    public int SectionId { get; set; }
    public virtual Sections Section { get; set; }
    public virtual PromotedTicket PromotedTicket { get; set; }

    #endregion
}
