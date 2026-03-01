
namespace Eshop.Domain.EntitiesConfiguration.OrderTicket;

public class PromotedTicketConfiguration : IEntityTypeConfiguration<PromotedTicket>
{
    public void Configure(EntityTypeBuilder<PromotedTicket> entity)
    {
        
        entity.Property(e => e.SectionTicketId).HasColumnName("SectionTicketID");
        entity.HasOne(d => d.SectionTicket)
            .WithMany(p => p.PromotedTickets)
            .HasForeignKey(d => d.SectionTicketId)
            .HasConstraintName("FK_SectionTickets_PromotedTickets");
    }
}
