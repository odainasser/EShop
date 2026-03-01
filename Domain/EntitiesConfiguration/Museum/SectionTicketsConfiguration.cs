
namespace Eshop.Domain.EntitiesConfiguration.Museum;

public class SectionTicketsConfiguration : IEntityTypeConfiguration<SectionTickets>
{
    // represent tickets for each section "museum".
    public void Configure(EntityTypeBuilder<SectionTickets> entity)
    {
        

        entity.Property(e => e.TicketId).HasColumnName("TicketID");
        entity.Property(e => e.SectionId).HasColumnName("SectionID");

        entity.Property(e => e.Discount)
            .HasDefaultValueSql("((0))");

        entity.Property(e => e.Vat)
            .HasColumnName("VAT");
      
        entity.HasOne(d => d.Section)
          .WithMany(p => p.SectionTickets)
          .HasForeignKey(d => d.SectionId)
          .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(d => d.Ticket)
          .WithMany(p => p.SectionTickets)
          .HasForeignKey(d => d.TicketId)
          .OnDelete(DeleteBehavior.Restrict);
    }
}
