namespace Eshop.Domain.Entities.OrderTicket;

public class IssuedTicketScanLogs : BaseEntity, IId<long>
{
    public long Id { get; set; }
   
    public string RefNumber { get; set; }
    public string? ExternalRefNumber { get; set; }
    public string TicketType { get; set; } // Basic - Groupon - Bustour - SharjahExperience ....
    public string TicketSource { get; set; } // Online - POS
    public long TicketReference { get; set; }
    public bool IsFree { get; set; } = false;
    public bool IsJoinedSections { get; set; } = false;
    public bool ForManyPersons { get; set; } = false;
    public bool IsExternal { get; set; } = false;
    public bool IsGroup { get; set; } = false;
    //public string GroupType { get; set; }
    public string AgeGroup { get; set; }
    public int? GroupCount { get; set; }
    public int? BabyCount { get; set; }
    public int? ChildCount { get; set; }
    public int? AdultCount { get; set; }

    public int? DisabilitiesCount { get; set; }
    public int? SeniorsCount { get; set; }
    public int? CompanionsCount { get; set; }

    public int UserSectionId { get; set; }
    public int ScannedSectionId { get; set; }

    /// <summary>
    /// related to POS
    /// </summary>
    public int? SalesNo { get; set; } // related to POS

    /// <summary>
    /// related to POS
    /// </summary>
    public int? POSNo { get; set; } // related to POS

    /// <summary>
    /// related to POS
    /// </summary>
    public string? StoreId { get; set; } // related to POS

    public int? UserId { get; set; }

    /// <summary>
    /// scan | cancel | renew | expired | ...
    /// </summary>
    public string ActionCode { get; set; } // scan | cancel | renew | expired | ...
    public int ActionBy { get; set; }
    public DateTime ActionAt { get; set; }

    public int? ScannedBy { get; set; }
    public string? ScannedByStaffId { get; set; }
    public string? Note { get; set; }

    public string? CreateByStaffId { get; set; }


    #region Relations

    public long IssuedTicketId { get; set; }
    public int SectionTicketId { get; set; }

    public virtual IssuedTickets IssuedTicket { get; set; }
    public virtual SectionTickets SectionTicket { get; set; }


    #endregion
}
