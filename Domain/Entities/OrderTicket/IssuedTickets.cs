namespace Eshop.Domain.Entities.OrderTicket;

public class IssuedTickets : BaseEntity, IId<long>
{
    public IssuedTickets()
    {
        IssuedTicketJoins = new HashSet<IssuedTicketJoins>();
        IssuedTicketScanLogs = new HashSet<IssuedTicketScanLogs>();
        IssuedTicketVisits = new HashSet<IssuedTicketVisits>();
    }

    public string? WifiCode { get; set; }

    public long Id { get; set; }
    public long TicketSeqRef { get; set; }
    public string RefNumber { get; set; }
    public string? ExternalRefNumber { get; set; }
    public string? UserGUID { get; set; }

    /// <summary>
    /// <see cref="EnumTicketSource"/>
    /// Online orders - promoted - POS
    /// </summary>
    public string TicketSource { get; set; } // Online orders - promoted - POS

    /// <summary>
    /// <see cref="EnumTicketBaseTypes"/>
    /// Basic - Groupon - Bustour - SharjahExperience ....
    /// </summary>
    public string TicketType { get; set; } // 

    public string ItemTahseelCode { get; set; }
    public string? ItemPOSTahseelCode { get; set; }

    /// <summary>
    /// <see cref="EnumOrderTicketPaymentStatus"/>
    /// </summary>
    public int PaymentStatus { get; set; } // Free | wait Pay | Payment Done
    public bool IsReservation { get; set; } = false;

    /// <summary>
    /// for 60+ years
    /// </summary>
    public bool ForSenior { get; set; } = false; // for 60+ years
    public bool WithCompanion { get; set; } = false; // morafeq
    public bool ForDisabilities { get; set; } = false;
    public bool IsFree { get; set; } = false;
    public bool IsJoinedSections { get; set; } = false;
    public bool ForManyPersons { get; set; } = false;
    public bool IsExternal { get; set; } = false;
    public bool IsPromoted { get; set; } = false; // refer to the free tickets issued by the system to marketing team.
    public bool IsForStaff { get; set; } = false; // ticket that will used for only staff
    public string? ForStaffId { get; set; }
    public bool IsManyVisits { get; set; } = false; // ticket that will used for multiple visits, like basic promotion [buy for three visits and get one visit free]
    public int? VisitsCount { get; set; }
    public bool SameDayUse { get; set; } // mean if ticket is used today, and it has period day limit more than one day, it cannot be used again after today.
    public bool HasPeriodLimit { get; set; } = false;
    public int PeriodDaysLimit { get; set; }
    public bool HasExpiration { get; set; } = false;
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public bool IsGroup { get; set; } = false;
    //public string GroupType { get; set; }
    public string AgeGroup { get; set; }
    public int? GroupCount { get; set; }
    public int? BabyCount { get; set; }
    public int? ChildCount { get; set; }
    public int? AdultCount { get; set; }
    public int? SeniorsCount { get; set; }
    public int? CompanionsCount { get; set; }
    public int? DisabilitiesCount { get; set; }
    public int? PaidGroupCount { get; set; }
    public int? MinGroupCount { get; set; }
    public int? MaxGroupCount { get; set; }
    public DateTime? AttendanceDate { get; set; }
    public DateTime? IssueDate { get; set; }

    /// <summary>
    /// تقريبا ان ممكن يحجز التذكره لاسبوع كمثال بس ده وقت الاستخدام الفعلي
    /// </summary>
    public DateTime? UsedAt { get; set; }
    public DateTime? FirstUsedAt { get; set; }
    public bool AutoScanned { get; set; } = false;
    public int? ScannedBy { get; set; }
    public string? ScannedByStaffId { get; set; }
    public string? Note { get; set; }
    public string StatusCode { get; set; }
    public bool IsExpired { get; set; } = false;
    public DateTime? ExpiredAt { get; set; }
    public bool NeedForceDelete { get; set; } = false;
    public string? CreateByStaffId { get; set; }

    public string? Equivalent_POS_type { get; set; }

    #region Relations

    /// <summary>
    /// see <see cref="OrderTickets"/> foreign key to <see cref="SectionTicket"/>
    /// and also <see cref="PaymentTransDetails.ItemReferenceId"/>
    /// or pos id
    /// </summary>
    public long TicketReference { get; set; }

    public int SectionTicketId { get; set; }
    public virtual SectionTickets SectionTicket { get; set; }

    /// <summary>
    /// if login
    /// </summary>
    public int? UserId { get; set; } // ex: user that make order
    public virtual ApplicationUser User { get; set; }

    #endregion

    #region Collections

    public virtual ICollection<IssuedTicketJoins> IssuedTicketJoins { get; set; }
    public virtual ICollection<IssuedTicketScanLogs> IssuedTicketScanLogs { get; set; }
    public virtual ICollection<IssuedTicketVisits> IssuedTicketVisits { get; set; }
   
    #endregion
}
