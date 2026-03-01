namespace Eshop.Domain.Entities.Museum;

/// <summary>
/// Museums
/// this contains all museums with data like titles , descriptions ,image ,.....
/// </summary>
public class Sections : BaseEntity, IId<int>
{
    public Sections()
    {
        SectionTranslations = new HashSet<SectionTranslations>();
        WorkingDays = new HashSet<SectionsWorkingDays>();
        AvailableQuantities = new HashSet<MuseumAvailableQuantities>();
        SectionTickets = new HashSet<SectionTickets>();
        Users = new HashSet<ApplicationUser>();
        SectionTicketJoins = new HashSet<SectionTicketJoins>();
        IssuedTicketVisits = new HashSet<IssuedTicketVisits>();
        IssuedTicketJoins = new HashSet<IssuedTicketJoins>();
        PromotedTicketJoins = new HashSet<PromotedTicketJoins>();
        POSTickets = new HashSet<POSTickets>();
        PosDevices = new HashSet<PosDevice>();
        SectionFacilities = new HashSet<SectionFacility>();
    }

    /// <summary>
    /// Primary Key
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// every museum has a code
    /// </summary>
    public string Code { get; set; }

    public string? POSCode { get; set; }

    public string? TahseelCode { get; set; }

    public string? POSTahseelCode { get; set; }

    /// <summary>
    /// <see cref="EnumSectionTypes"/>
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// url of an image to show the museum main image to describe it.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Lat , long
    /// </summary>
    public string? LocCoordinates { get; set; } // Location Coordinates

    /// <summary>
    /// every department should have 1 main section.
    /// </summary>
    public bool IsMain { get; set; } = false;


    /// <summary>
    /// Number of Persons that can ticket per hour
    /// because in corona it set to 150 person per hour
    /// not it's 1500
    /// </summary>
    public int PersPerHourBooking { get; set; } // number of persons per hour that can book museums ticket online.

    /// <summary>
    /// museum may be closed for modification , maintenaance ,....
    /// no tickets if it is true
    /// </summary>
    public bool IsClosed { get; set; } = false;

    #region Relations

    public int DepartmentId { get; set; }

    public virtual Departments Department { get; set; }

    #endregion

    #region Collections

    /// <summary>
    /// Contains Museum Title ar , en , description ar,en ,.....
    /// </summary>
    [TranslationsSet]
    public virtual ICollection<SectionTranslations> SectionTranslations { get; set; }

    /// <summary>
    /// Contains Museum Facilty has wifi , Lift, .....
    /// </summary>
    public virtual ICollection<SectionFacility> SectionFacilities { get; set; }

    public virtual ICollection<SectionTickets> SectionTickets { get; set; }

    public virtual ICollection<ApplicationUser> Users { get; set; }
    public virtual ICollection<SectionsWorkingDays> WorkingDays { get; set; }
    public virtual ICollection<MuseumAvailableQuantities> AvailableQuantities { get; set; }
    public virtual ICollection<SectionTicketJoins> SectionTicketJoins { get; set; }
    public virtual ICollection<IssuedTicketJoins> IssuedTicketJoins { get; set; }
    public virtual ICollection<IssuedTicketVisits> IssuedTicketVisits { get; set; }
    public virtual ICollection<PromotedTicketJoins> PromotedTicketJoins { get; set; }
    public virtual ICollection<POSTickets> POSTickets { get; set; }
    public virtual ICollection<PosDevice> PosDevices { get; set; }
    public virtual ICollection<SectionWifiSetting> SectionWifiSettings { get; set; }

    #endregion
}

/// <summary>
/// contains title , description,.. in arabic and english
/// </summary>
/// <param name="lazyLoader"></param>
[TranslationFor(typeof(Sections))]
public class SectionTranslations : TranslatedEntity, IId<int>, ISectionEntity
{
    public int Id { get; set; }
    public int LanguageId { get; set; }

    public string Title { get; set; }
    public string? Desc { get; set; }

    public string? LocationDescription { get; set; }
    public string? VisitTimesDescription { get; set; }

    #region Relations

    public int SectionId { get; set; }
    public virtual Sections Section { get; set; }

    #endregion
}

/// <summary>
/// Contains Museum Facilty has wifi , Lift, .....
/// </summary>
[Table(nameof(SectionFacility) + "s", Schema = SchemaConstant.Museum)]
public class SectionFacility : IId<int>, ISectionEntity
{
    public int Id { get; set; }

    #region Relations

    [ForeignKey(nameof(SectionId))]
    public virtual Sections Section { get; set; }

    public int SectionId { get; set; }

    [ForeignKey(nameof(FacilityId))]
    public virtual Facility Facility { get; set; }

    public int FacilityId { get; set; }

    #endregion
}