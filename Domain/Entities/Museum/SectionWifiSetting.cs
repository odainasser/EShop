namespace Eshop.Domain.Entities.Museum;

public class SectionWifiSetting : BaseEntity, ISectionEntity, IId<int>
{
    public int Id { get; set; }

    public int SectionId { get; set; }

    /// <summary>
    /// from connected service ipera Wi-Fi IWifApiService.GetLocations
    /// </summary>
    public string? LocationId { get; set; }

    /// <summary>
    /// from connected service ipera Wi-Fi IWifApiService.GetServiceProfiles
    /// </summary>
    public string? ServiceProfile { get; set; }

    public virtual Sections Section { get; set; }
}