namespace Eshop.Domain.Entities.OrderTicket;

[AuditableEntity]
public class PromotedTicket : BaseEntity, IId<int>
{
    // refer to the free tickets issued by the system to marketing team.

    public PromotedTicket()
    {
        PromotedTicketJoins = new HashSet<PromotedTicketJoins>();
    }

    public int Id { get; set; }
  
    public int Quantity { get; set; } // how many free tickets should be issued.

    public bool IsJoinedSections { get; set; } = false;
    public bool IsGroup { get; set; } = false;
    public string Description { get; set; }
    public string GroupType { get; set; }
    public string AgeGroup { get; set; }
    public int GroupCount { get; set; }
    public bool HasExpiration { get; set; } = false;
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }

    #region Relations

    public int SectionTicketId { get; set; }
    public virtual SectionTickets SectionTicket { get; set; }

    #endregion

    #region Collections

    public virtual ICollection<PromotedTicketJoins> PromotedTicketJoins { get; set; }

    #endregion
}
