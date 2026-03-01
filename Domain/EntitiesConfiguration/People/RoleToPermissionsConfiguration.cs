namespace Eshop.Domain.EntitiesConfiguration.People;

public class RoleToPermissionsConfiguration : IEntityTypeConfiguration<RoleToPermissions>
{
    public void Configure(EntityTypeBuilder<RoleToPermissions> entity)
    {
        entity.Property(e => e.Id).HasColumnName("ID");
        entity.Property("_permissionsInRole").HasColumnName("PermissionsInRole");
    }
}


public class RoleToPermissionsTranslationsConfiguration : IEntityTypeConfiguration<RoleToPermissionsTranslations>
{
    public void Configure(EntityTypeBuilder<RoleToPermissionsTranslations> entity)
    {
        

        entity.Property(e => e.Desc).HasMaxLength(500);

        entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

        entity.Property(e => e.Title).HasMaxLength(500);

        entity.Property(e => e.RoleToPermissionsId).HasColumnName("RoleToPermissionsId");

        entity.HasOne(d => d.Language)
            .WithMany(p => p.RoleToPermissionsTranslations)
            .HasForeignKey(d => d.LanguageId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_RoleToPermissionsTranslations_Languages");

        entity.HasOne(d => d.RoleToPermissions)
            .WithMany(p => p.RoleToPermissionsTranslations)
            .HasPrincipalKey(d => d.Id)
            .HasForeignKey(d => d.RoleToPermissionsId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_RoleToPermissionsTranslations_RoleToPermissions");
    }
}
