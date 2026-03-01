
namespace Eshop.Domain.EntitiesConfiguration.Lukups;

public class AppModulesConfiguration : IEntityTypeConfiguration<AppModules>
{
    public void Configure(EntityTypeBuilder<AppModules> entity)
    {
        entity.HasIndex(e => e.Code)
              .HasName("ICode_AppModules")
              .IsUnique();

        entity.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(250);

        entity.Property(e => e.DescAr).HasMaxLength(500);
        entity.Property(e => e.DescEn).HasMaxLength(500);
        entity.Property(e => e.NameAr).HasMaxLength(500);
        entity.Property(e => e.NameEn).HasMaxLength(500);
    }
}
