namespace Eshop.Domain.Entities.Carts;

/// <summary>
/// User Cart Tickets
/// </summary>
[ProjectType(ProjectTypeEnum.EshopWeb | ProjectTypeEnum.EshopApi)]
public class CartTickets : BaseEntity, IId<int>
{
    public int Id { get; set; }

    public string? ItemTahseelCode { get; set; }
    public bool IsHolded { get; set; } = false;
    public bool IsJoinedSections { get; set; } = false;
    public bool IsGroup { get; set; } = false;
    public string AgeGroup { get; set; }
    public int MinGroupCount { get; set; } = 0;
    public int MaxGroupCount { get; set; } = 0;
    public int GroupCount { get; set; } = 0;
    public int BabyCount { get; set; } = 0;
    public int ChildCount { get; set; } = 0;
    public int AdultCount { get; set; } = 0;
    public DateTime? AttendanceDate { get; set; }
    public decimal UnitPrice { get; set; } = 0;
    public decimal UnitVat { get; set; } = 0;
    public int Qty { get; set; } = 0;
    public decimal TotalAmount { get; set; } = 0;
    public decimal TotalVat { get; set; } = 0;
    public decimal Discount { get; set; } = 0;
    public bool IsFree { get; set; } = false;

    /// <summary>
    /// بص كان فيه قبل كد ان فيه عدد محدد ف اليوم للتذاكر بناءا علي كل متحف فلو حد ضاف تذكره
    ///من غير ما يدفع يعني ضاف ف السله فقط بنحفظها لمده معينه فقط وده تاريخ الاضافة
    /// </summary>
    public DateTime? HoldedAt { get; set; }
    public DateTime? HoldingExpiredAt { get; set; }

    #region Relations

    public int CartId { get; set; }
    public int? UserId { get; set; }
    public int SectionTicketId { get; set; }

    public virtual Cart Cart { get; set; }
    public virtual SectionTickets SectionTicket { get; set; }
    public virtual ApplicationUser User { get; set; }

    #endregion
}