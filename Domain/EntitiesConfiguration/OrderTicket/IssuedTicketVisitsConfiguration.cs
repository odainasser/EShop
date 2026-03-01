
namespace Eshop.Domain.EntitiesConfiguration.OrderTicket;

public class IssuedTicketVisitsConfiguration : IEntityTypeConfiguration<IssuedTicketVisits>
{
    public void Configure(EntityTypeBuilder<IssuedTicketVisits> entity)
    {
        
        entity.Property(e => e.IssuedTicketId).HasColumnName("IssuedTicketID");
        entity.Property(e => e.SectionId).HasColumnName("SectionID");

        entity.HasOne(d => d.Section)
            .WithMany(p => p.IssuedTicketVisits)
            .HasForeignKey(d => d.SectionId);

        entity.HasOne(d => d.IssuedTicket)
            .WithMany(p => p.IssuedTicketVisits)
            .HasForeignKey(d => d.IssuedTicketId);
    }
}
