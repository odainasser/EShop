
namespace Eshop.Domain.EntitiesConfiguration.Lukups;

public class SiteVisitTicketsConfiguration : IEntityTypeConfiguration<SiteVisitTickets>
{
    public void Configure(EntityTypeBuilder<SiteVisitTickets> entity)
    {
        entity.Property(e => e.SiteVisitId).HasColumnName("SiteVisitID");
        entity.Property(e => e.SectionTicketId).HasColumnName("SectionTicketID");
        
        entity.HasOne(d => d.SiteVisit)
           .WithMany(p => p.SiteVisitTickets)
           .HasPrincipalKey(d => d.Id)
           .HasForeignKey(d => d.SiteVisitId)
           .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(d => d.SectionTicket)
           .WithMany(p => p.SiteVisitTickets)
           .HasPrincipalKey(d => d.Id)
           .HasForeignKey(d => d.SectionTicketId)
           .OnDelete(DeleteBehavior.Cascade);
    }
}
