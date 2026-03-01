
namespace Eshop.Domain.EntitiesConfiguration.Museum;

public class MuseumAvailableQuantitiesConfiguration : IEntityTypeConfiguration<MuseumAvailableQuantities>
{
    public void Configure(EntityTypeBuilder<MuseumAvailableQuantities> entity)
    {
        entity.Property(e => e.SectionId).HasColumnName("SectionID");

        entity.HasOne(d => d.Section)
            .WithMany(p => p.AvailableQuantities)
            .HasForeignKey(d => d.SectionId)
            .HasPrincipalKey(e => e.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Sections_MuseumAvailableQuantities");
    }
}
