
namespace Eshop.Domain.EntitiesConfiguration.Lukups;

public class AppLookupTypesConfiguration : IEntityTypeConfiguration<AppLookupTypes>
{
    public void Configure(EntityTypeBuilder<AppLookupTypes> entity)
    {
        entity.HasIndex(e => e.Code)
               .HasName("ICode_AppLookupTypes")
               .IsUnique();
        entity.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(250);
        entity.Property(e => e.DescAr).HasMaxLength(500).IsRequired(false);
        entity.Property(e => e.DescEn).HasMaxLength(500).IsRequired(false);
        entity.Property(e => e.NameAr).HasMaxLength(500).IsRequired(false);
        entity.Property(e => e.NameEn).HasMaxLength(500).IsRequired(false);
    }
}
