
namespace Eshop.Domain.EntitiesConfiguration.OrderTicket;

public class POSTicketDetailsConfiguration : IEntityTypeConfiguration<POSTicketDetails>
{
    public void Configure(EntityTypeBuilder<POSTicketDetails> entity)
    {
        
        entity.Property(e => e.POSTicketId).HasColumnName("POSTicketID");
        entity.HasOne(d => d.POSTicket)
           .WithMany(p => p.POSTicketDetails)
           .HasPrincipalKey(d => d.Id)
           .HasForeignKey(d => d.POSTicketId)
           .OnDelete(DeleteBehavior.Cascade)
           .HasConstraintName("FK_POSTicketDetails_POSTickets");

        entity.HasOne(d => d.SectionTicket)
           .WithMany(p => p.POSTicketDetails)
           .OnDelete(DeleteBehavior.Restrict)
           .HasConstraintName("FK_POSTicketDetails_SectionTickets");
    }
}
