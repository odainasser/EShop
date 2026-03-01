namespace Eshop.Domain.Entities.POS;

public class PosDevice : BaseEntity, IId<int>, ISectionEntity
{
    public int Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Computer or device(that shows the Website) IP
    /// </summary>
    public string IP_Address { get; set; }

    /// <summary>
    /// printer name
    /// </summary>
    public string PrinterName { get; set; }

    /// <summary>
    /// printer ip
    /// </summary>
    public string? PrinterIP { get; set; }

    /// <summary>
    /// pay device terminal id
    /// open the pay device and look for TID on the screen
    /// </summary>
    public string? TerminalNo { get; set; }

    #region Relations

    [ForeignKey(nameof(Section))]
    public int SectionId { get; set; }

    public Sections Section { get; set; }

    #endregion
}
