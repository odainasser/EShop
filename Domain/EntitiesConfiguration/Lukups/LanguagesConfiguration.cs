
namespace Eshop.Domain.EntitiesConfiguration.Lukups;

public class LanguagesConfiguration : IEntityTypeConfiguration<Languages>
{
    public void Configure(EntityTypeBuilder<Languages> entity)
    {
        entity.Property(e => e.IsoCode)
            .IsRequired()
            .HasMaxLength(5);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(250);
    }
}
