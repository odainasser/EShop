
namespace Eshop.Domain.EntitiesConfiguration.OrderTicket;

public class IssuedTicketsConfiguration : IEntityTypeConfiguration<IssuedTickets>
{
    public void Configure(EntityTypeBuilder<IssuedTickets> entity)
    {
        
        entity.Property(e => e.SectionTicketId).HasColumnName("SectionTicketID");

        entity.Property(e => e.RefNumber).HasMaxLength(500);
      
        entity.Property(e => e.StatusCode)
            .IsRequired()
            .HasMaxLength(250)
            .HasDefaultValue("0");

        entity.HasOne(d => d.SectionTicket)
            .WithMany(p => p.IssuedTickets)
            .HasForeignKey(d => d.SectionTicketId)
            .HasConstraintName("FK_SectionTickets_IssuedTickets");

        entity.HasOne(d => d.User)
            .WithMany(p => p.IssuedTickets)
            .HasPrincipalKey(p => p.UserId)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FK_IssuedTickets_AspNetUsers");
        entity.Property(o => o.TicketSeqRef)
            .HasDefaultValueSql("NEXT VALUE FOR shared.TicketsNumbers");
    }
}
