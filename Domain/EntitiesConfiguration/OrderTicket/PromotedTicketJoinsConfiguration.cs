
namespace Eshop.Domain.EntitiesConfiguration.OrderTicket;

public class PromotedTicketJoinsConfiguration : IEntityTypeConfiguration<PromotedTicketJoins>
{
    public void Configure(EntityTypeBuilder<PromotedTicketJoins> entity)
    {
        
        entity.Property(e => e.PromotedTicketId).HasColumnName("PromotedTicketID");
  
        entity.HasOne(d => d.Section)
            .WithMany(p => p.PromotedTicketJoins)
            .HasForeignKey(d => d.SectionId);

        entity.HasOne(d => d.PromotedTicket)
            .WithMany(p => p.PromotedTicketJoins)
            .HasForeignKey(d => d.PromotedTicketId);
    }
}
