
namespace Eshop.Domain.EntitiesConfiguration.Museum;

public class SectionsWorkingDaysConfiguration : IEntityTypeConfiguration<SectionsWorkingDays>
{
    public void Configure(EntityTypeBuilder<SectionsWorkingDays> entity)
    {
        entity.Property(e => e.SectionId).HasColumnName("SectionID");
        entity.HasOne(d => d.Section)
            .WithMany(p => p.WorkingDays)
            .HasForeignKey(d => d.SectionId)
            .HasPrincipalKey(e => e.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Sections_SectionsWorkingDays");
    }
}
