
namespace Eshop.Domain.EntitiesConfiguration.Museum;

public class DepartmentsConfiguration : IEntityTypeConfiguration<Departments>
{
    public void Configure(EntityTypeBuilder<Departments> entity)
    {
        entity.Property(e => e.Code).IsRequired();
        entity.HasIndex(e => e.Code).IsUnique();
    }
}


public class DepartmentTranslationsConfiguration : IEntityTypeConfiguration<DepartmentTranslations>
{
    public void Configure(EntityTypeBuilder<DepartmentTranslations> entity)
    {
        

        entity.Property(e => e.Title).HasMaxLength(500);
        entity.Property(e => e.Desc).HasMaxLength(500);

        entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

        entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

        entity.HasOne(d => d.Language)
            .WithMany(p => p.DepartmentTranslations)
            .HasForeignKey(d => d.LanguageId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_DepartmentTranslations_Languages");

        entity.HasOne(d => d.Department)
            .WithMany(p => p.DepartmentTranslations)
            .HasForeignKey(d => d.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_DepartmentTranslations_Departments");
    }
}
