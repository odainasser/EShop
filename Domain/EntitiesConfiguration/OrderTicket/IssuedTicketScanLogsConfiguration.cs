
namespace Eshop.Domain.EntitiesConfiguration.OrderTicket;

public class IssuedTicketScanLogsConfiguration : IEntityTypeConfiguration<IssuedTicketScanLogs>
{
    public void Configure(EntityTypeBuilder<IssuedTicketScanLogs> entity)
    {
        

        entity.Property(e => e.IssuedTicketId).HasColumnName("IssuedTicketID");
        entity.Property(e => e.SectionTicketId).HasColumnName("SectionTicketID");
        entity.Property(e => e.ScannedSectionId).HasColumnName("ScannedSectionID");
        entity.Property(e => e.UserSectionId).HasColumnName("UserSectionID");
        entity.Property(e => e.UserId).HasColumnName("UserID");
        
        entity.HasOne(d => d.IssuedTicket)
            .WithMany(p => p.IssuedTicketScanLogs)
            .HasForeignKey(d => d.IssuedTicketId);

        entity.HasOne(d => d.SectionTicket)
            .WithMany(p => p.IssuedTicketScanLogs)
            .HasForeignKey(d => d.SectionTicketId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
