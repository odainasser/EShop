namespace Eshop.Domain.EntitiesConfiguration.OrderTicket;

public class TicketCategoriesConfiguration : IEntityTypeConfiguration<TicketCategories>
{
    public void Configure(EntityTypeBuilder<TicketCategories> entity)
    {
        
        entity.Property(e => e.Code).IsRequired();
        entity.HasIndex(e => e.Code).IsUnique();
    }
}
public class TicketCategoryTranslationsConfiguration : IEntityTypeConfiguration<TicketCategoryTranslations>
{
    public void Configure(EntityTypeBuilder<TicketCategoryTranslations> entity)
    {
        

        entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

        entity.Property(e => e.Name).HasMaxLength(500);

        entity.Property(e => e.TicketCategoryId).HasColumnName("TicketCategoryID");

        entity.HasOne(d => d.Language)
            .WithMany(p => p.TicketCategoryTranslations)
            .HasForeignKey(d => d.LanguageId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_TicketCategoryTranslations_Languages");

        entity.HasOne(d => d.TicketCategory)
            .WithMany(p => p.TicketCategoryTranslations)
            .HasForeignKey(d => d.TicketCategoryId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_TicketCategoryTranslations_TicketCategories");
    }
}