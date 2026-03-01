namespace Eshop.Domain.Entities.People;

/// <summary>
/// This holds each Role, which are mapped to Permissions
/// </summary>
[AuditableEntity]
public class RoleToPermissions : BaseEntity, IId<int>
{
    [Required(AllowEmptyStrings = false)] //A role must have at least one role in it
    private string _permissionsInRole;

    public RoleToPermissions()
    {
        RoleToPermissionsTranslations = new HashSet<RoleToPermissionsTranslations>();
        UserToRoles = new HashSet<UserToRole>();
    }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// ShortName of the role
    /// </summary>
    [Key, Required(AllowEmptyStrings = false)]
    [MaxLength(ExtraAuthConstants.RoleNameSize)]
    public string RoleName { get; set; }
    public bool ForExternalApp { get; set; }

    /// <summary>
    /// A human-friendly description of what the Role does
    /// </summary>
    //[Required(AllowEmptyStrings = false)]
    //public string Description { get; private set; }

    [TranslationsSet]
    public virtual ICollection<RoleToPermissionsTranslations> RoleToPermissionsTranslations { get; set; }

    public virtual ICollection<UserToRole> UserToRoles { get; set; }

    /// <summary>
    /// This returns the list of permissions in this role
    /// </summary>
    public IEnumerable<Permissions> PermissionsInRole => _permissionsInRole.UnpackPermissionsFromString();

    public void UpdatePermissions(ICollection<Permissions> permissions)
    {
        if (permissions == null || !permissions.Any())
            throw new InvalidOperationException("There should be at least one permission associated with a role.");

        _permissionsInRole = permissions.PackPermissionsIntoString();
        //Description = description;
    }
}