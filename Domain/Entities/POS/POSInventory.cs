// ReSharper disable InconsistentNaming
namespace Eshop.Domain.Entities.POS;

public class POSInventory : BaseEntity, IId<Guid>, ISectionEntity
{
    public Guid Id { get; set; }
    public string PosCode { get; set; }
    public int SectionId { get; set; }
    public string TransactionDate { get; set; }
    public decimal MinusVal { get; set; }
    public decimal PlusVal { get; set; }
    public decimal CashBoxVal { get; set; }
    public decimal NewCashBoxVal { get; set; }
    public decimal RecivedVal { get; set; }
    public decimal TotalFees { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal TotalService { get; set; }
    public decimal NetVal { get; set; }
    public decimal NetPosInventory { get; set; }
    public decimal PrIn { get; set; }
    public decimal PrOut { get; set; }

}
