namespace Eshop.Domain.Entities.Museum;

public class SectionTicketJoins : BaseEntity, IId<int>, ISectionEntity
{
    public int Id { get; set; }

    #region Relations

    public int SectionTicketId { get; set; }
    public int SectionId { get; set; }

    public virtual Sections Section { get; set; }
    public virtual SectionTickets SectionTicket { get; set; }

    #endregion
}
