namespace Eshop.Domain.EntitiesConfiguration.Museum;

public class SectionTransaltionConfiguration : IEntityTypeConfiguration<SectionTranslations>
{
    public void Configure(EntityTypeBuilder<SectionTranslations> entity)
    {
        

        entity.Property(e => e.Title).HasMaxLength(500);
        entity.Property(e => e.Desc).HasMaxLength(500);

        entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

        entity.Property(e => e.SectionId).HasColumnName("SectionID");

        entity.HasOne(d => d.Language)
            .WithMany(p => p.SectionTranslations)
            .HasForeignKey(d => d.LanguageId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_SectionTranslations_Languages");

        entity.HasOne(d => d.Section)
            .WithMany(p => p.SectionTranslations)
            .HasForeignKey(d => d.SectionId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_SectionTranslations_Sections");
    }
}
