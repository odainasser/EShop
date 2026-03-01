
namespace Eshop.Domain.EntitiesConfiguration.OrderTicket;

public class OrderTicketsConfiguration : IEntityTypeConfiguration<OrderTickets>
{
    public void Configure(EntityTypeBuilder<OrderTickets> entity)
    {
        
        entity.Property(e => e.SectionTicketId).HasColumnName("SectionTicketID");

        entity.Property(e => e.PaymentStatus).HasDefaultValue(1);
        
        entity.Property(e => e.Discount)
            .HasDefaultValueSql("((0))");

        entity.Property(e => e.TotalVat)
            .HasColumnName("TotalVAT");

        entity.HasOne(d => d.Order)
            .WithMany(p => p.OrderTickets)
            .HasForeignKey(d => d.OrderId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_OrderTickets_Orders");

        entity.HasOne(d => d.SectionTicket)
            .WithMany(p => p.OrderTickets)
            .HasForeignKey(d => d.SectionTicketId)
            .HasConstraintName("FK_OrderTickets_SectionTickets");
    }
}
