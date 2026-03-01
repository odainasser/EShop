
namespace Eshop.Domain.EntitiesConfiguration.OrderTicket;

public class POSTicketsConfiguration : IEntityTypeConfiguration<POSTickets>
{
    public void Configure(EntityTypeBuilder<POSTickets> entity)
    {
        
        entity.Property(e => e.SectionId).HasColumnName("SectionID");

      
        entity.Property(e => e.TicketRefNumber).HasMaxLength(500);
        entity.Property(e => e.ExternalRefNumber).HasMaxLength(500);
        entity.Property(e => e.PaymentMethod).HasDefaultValue("1");

        entity.HasOne(d => d.Section)
            .WithMany(p => p.POSTickets)
            .HasForeignKey(d => d.SectionId)
            .HasPrincipalKey(e => e.Id)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired()
            .HasConstraintName("FK_Sections_POSTickets");

        entity.HasOne(d => d.SectionTicket)
          .WithMany(p => p.POSTickets)
          .HasForeignKey(d => d.SectionTicketId)
          .HasPrincipalKey(e => e.Id)
          .OnDelete(DeleteBehavior.Restrict)
          .IsRequired()
          .HasConstraintName("FK_SectionTickets_POSTickets");
    }
}
