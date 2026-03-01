

namespace Eshop.Domain.Entities.People;

public class AnnualMembership : BaseEntity, IId<int>
{
    //public AnnualMembership()
    //{
    //    Users = new HashSet<ApplicationUser>();
    //}
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? BirthDate { get; set; } = null;
    public DateTime? StartDate { get; set; } = null;
    public DateTime? EndDate { get; set; } = null;
    public string? PhoneNumber { get; set; } = null;
    public string? OrderReference { get; set; } = null;
    public string? Email { get; set; } = null;
    public string? MembershipNumber { get; set; } = null;
    public int? Type { get; set; } = null;
    public int? Status { get; set; } = null;
    public int? Gender { get; set; } = null;
    public bool? IsFullyPaid { get; set; } = null;

    public int CardCollectionMethod { get; set; } = 1;
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Area { get; set; }
    public string? StreetNameNumber { get; set; }
    public string? ApartmentVillaOffice { get; set; }
    public string? NearestLandmark { get; set; }
    public string? ExtraInformation { get; set; }
    public string? AddressPhoneNumber { get; set; }



    public bool IsSynced { get; set; }

    #region Relation

    public int UserId { get; set; }
    // public virtual ICollection<ApplicationUser> Users { get; set; }
    public virtual ApplicationUser User { get; set; }

    #endregion
}


