
namespace Eshop.Domain.EntitiesConfiguration.People;

public class UserToRoleConfiguration : IEntityTypeConfiguration<UserToRole>
{
    public void Configure(EntityTypeBuilder<UserToRole> entity)
    {
        entity.HasKey(x => new { x.UserId, x.RoleName });

        entity.Property(e => e.UserId).HasMaxLength(450);

        entity.HasOne(d => d.Role)
            .WithMany(p => p.UserToRoles)
            .HasForeignKey(d => d.RoleName)
            .HasPrincipalKey(r => r.RoleName)
            .HasConstraintName("FK_UserToRoles_RolesToPermissions_RoleName")
            .OnDelete(DeleteBehavior.NoAction);

        entity.HasOne(d => d.User)
            .WithMany(p => p.UserToRoles)
            .HasForeignKey(d => d.UserId)
            .HasPrincipalKey(r => r.Id)
            .HasConstraintName("FK_UserToRoles_AspNetUsers_UserId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
