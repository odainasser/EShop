namespace Eshop.Domain.Entities.Carts;

/// <summary>
/// User Cart In E-shop only
/// </summary>
[ProjectType(ProjectTypeEnum.EshopApi | ProjectTypeEnum.EshopWeb)]
public class Cart : BaseEntity, IId<int>
{
    public int Id { get; set; }

    public decimal TotalAmount { get; set; } = 0;

    public decimal TotalVat { get; set; } = 0;

    /// <summary>
    /// <see cref="EnumCartStatus"/>
    /// </summary>
    public string StatusCode { get; set; } = string.Empty;

    #region Relation

    public int UserId { get; set; }
    public virtual ApplicationUser User { get; set; }

    #endregion

    #region Collections

    public virtual ICollection<CartTickets> CartTickets { get; set; }
  
    #endregion
}
