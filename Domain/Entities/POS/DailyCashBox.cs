namespace Eshop.Domain.Entities.POS;

public class DailyCashBox : BaseEntity, IId<Guid>, ISectionEntity
{
    public Guid Id { get; set; }

    [Required]
    public int SectionId { get; set; }

    [Required]
    public string PosCode { get; set; } = string.Empty;

    public decimal CashBoxValue { get; set; }

    public DateTime TransactionDate { get; set; }

    public bool Posted { get; set; }
}
