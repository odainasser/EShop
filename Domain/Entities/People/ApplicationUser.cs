namespace Eshop.Domain.Entities.People;

[AuditableEntity]
public class ApplicationUser : IdentityUser, IBaseEntity
{
    public ApplicationUser()
    {
        RefreshTokens = new HashSet<RefreshToken>();
        Cart = new HashSet<Cart>();
        CartTickets = new HashSet<CartTickets>();
        Orders = new HashSet<Orders>();
        IssuedTickets = new HashSet<IssuedTickets>();
        UserToRoles = new HashSet<UserToRole>();
        SiteVisits = new HashSet<SiteVisits>();
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? Gender { get; set; }
    public DateTime? BirthDate { get; set; } = null;

    public string? Nationality { get; set; }
    public string? CountryOfResidence { get; set; }


    public string? ImageUrl { get; set; }
    public bool IsAdmin { get; set; } = false;
    public bool IsDeveloper { get; set; } = false;
    public bool IsExternalApp { get; set; } = false;

    public string? StaffId { get; set; }
    public string? NormalizedStaffId { get; set; }

    public int? ReferenceId { get; set; }

    private string? _permissionsForUser;

    /// <summary>
    /// This returns the list of permissions for this user
    /// </summary>
    public IEnumerable<Permissions>? PermissionsForUser => _permissionsForUser.UnpackPermissionsFromString();

    public void UpdatePermissions(ICollection<Permissions> permissions = null)
    {
        string _permissions = null;

        if (permissions != null && permissions.Any())
        {
            _permissions = permissions.PackPermissionsIntoString();
        }

        _permissionsForUser = _permissions;
    }

    public bool IsActive { get; set; } = true;
    public int? CreateBy { get; set; }
    public DateTime CreateAt { get; set; }
    public int? EditBy { get; set; }
    public DateTime? EditAt { get; set; }
    public int? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    #region Not Mapped

    [NotMapped]
    public string printerName { get; set; }
    [NotMapped]
    public string printerIp { get; set; }
    [NotMapped]
    public string posCode { get; set; }

    [NotMapped]
    public string terminalNo { get; set; }

    #endregion

    public bool IsSynced { get; set; }

    #region Relations

    public int? UsersTitleId { get; set; }
    public int? SectionId { get; set; }

    [ForeignKey(nameof(SectionId))]
    public virtual Sections Section { get; set; }


    [ForeignKey(nameof(UsersTitleId))]
    public virtual UsersTitles UserTitle { get; set; }

    #endregion

    #region Collections

    [ForeignKey("UserId")]
    public ICollection<RefreshToken> RefreshTokens { get; set; }
    public virtual ICollection<Cart> Cart { get; set; }
    public virtual ICollection<CartTickets> CartTickets { get; set; }
    public virtual ICollection<Orders> Orders { get; set; }
    public virtual ICollection<IssuedTickets> IssuedTickets { get; set; }
    public virtual ICollection<UserToRole> UserToRoles { get; set; }
    public virtual ICollection<SiteVisits> SiteVisits { get; set; }
    public virtual ICollection<AnnualMembership> AnnualMembership { get; set; }

    #endregion
}
