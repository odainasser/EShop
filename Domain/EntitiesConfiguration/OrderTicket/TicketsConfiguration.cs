
namespace Eshop.Domain.EntitiesConfiguration.OrderTicket;

public class TicketsConfiguration : IEntityTypeConfiguration<Tickets>
{
    public void Configure(EntityTypeBuilder<Tickets> entity)
    {
        
        entity.Property(e => e.TicketCategoryId).HasColumnName("TicketCategoryID");
        entity.Property(e => e.ImageUrl).HasMaxLength(500);

        entity.Property(e => e.Discount)
            .HasDefaultValueSql("((0))");

        entity.HasOne(d => d.TicketCategory)
           .WithMany(p => p.Tickets)
           .HasForeignKey(d => d.TicketCategoryId)
           .HasPrincipalKey(e => e.Id)
           .IsRequired()
           .OnDelete(DeleteBehavior.Restrict)
           .HasConstraintName("FK_Tickets_TicketCategories");
    }
}
public class TicketTranslationsConfiguration : IEntityTypeConfiguration<TicketTranslations>
{
    public void Configure(EntityTypeBuilder<TicketTranslations> entity)
    {
        

        entity.Property(e => e.Desc).HasMaxLength(500);

        entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

        entity.Property(e => e.Name).HasMaxLength(500);

        entity.Property(e => e.TicketId).HasColumnName("TicketID");

        entity.HasOne(d => d.Language)
            .WithMany(p => p.TicketTranslations)
            .HasForeignKey(d => d.LanguageId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_TicketTranslations_Languages");

        entity.HasOne(d => d.Ticket)
            .WithMany(p => p.TicketTranslations)
            .HasForeignKey(d => d.TicketId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_TicketTranslations_Tickets");
    }
}