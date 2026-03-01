namespace Eshop.Domain.Entities.People;

/// <summary>
/// This is a one-to-many relationship between the User (represented by the UserId) and their Roles (represented by RoleToPermissions)
/// </summary>
public class UserToRole : BaseEntity
{
    public UserToRole() { } //needed by EF Core

    public UserToRole(string userId, RoleToPermissions role)
    {
        UserId = userId;
        Role = role;
    }

    //I use a composite key for this table: combination of UserId and RoleName
    //That has to be defined by EF Core's fluent API
    [Required(AllowEmptyStrings = false)]
    [MaxLength(ExtraAuthConstants.UserIdSize)]
    public string UserId { get; private set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(ExtraAuthConstants.RoleNameSize)]
    public string RoleName { get; private set; }

    [ForeignKey(nameof(RoleName))]
    public RoleToPermissions Role { get; set; }

    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; private set; }
}