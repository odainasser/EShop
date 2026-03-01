namespace Eshop.Domain.EntitiesConfiguration.People;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> entity)
    {

        //Make it here unique_index_PROP
        entity.HasIndex(e => e.ReferenceId).IsUnique();

        entity.Property(e => e.BirthDate).HasColumnType("date");

        entity.Property(e => e.Gender).HasColumnType("int").HasColumnName("Gender");
        entity.Property(e => e.Nationality).HasColumnName("Nationality");
        entity.Property(e => e.CountryOfResidence).HasColumnName("CountryOfResidence");

        entity.Property(e => e.UsersTitleId).HasColumnName("UsersTitleID");
        entity.Property(e => e.SectionId).HasColumnName("SectionID");

        entity.Property(e => e.StaffId).HasColumnName("StaffID");
        entity.Property(e => e.NormalizedStaffId).HasColumnName("NormalizedStaffID");

        entity.Property("_permissionsForUser")
            .HasColumnName("PermissionsForUser")
            .HasColumnType("nvarchar(MAX)");

        entity.HasOne(d => d.Section)
            .WithMany(p => p.Users)
            .HasForeignKey(d => d.SectionId)
            .HasPrincipalKey(pk => pk.Id)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_AspNetUsers_Sections");

        entity.HasOne(d => d.UserTitle)
            .WithMany(p => p.Users)
            .HasForeignKey(d => d.UsersTitleId)
            .HasPrincipalKey(pk => pk.Id)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_AspNetUsers_UsersTitles");
    }
}