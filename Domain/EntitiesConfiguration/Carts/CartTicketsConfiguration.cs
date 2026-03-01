
namespace Eshop.Domain.EntitiesConfiguration.Carts;

public class CartTicketsConfiguration : IEntityTypeConfiguration<CartTickets>
{
    public void Configure(EntityTypeBuilder<CartTickets> entity)
    {
        entity.Property(e => e.SectionTicketId).HasColumnName("SectionTicketID");
        entity.Property(e => e.Discount)
            .HasDefaultValueSql("((0))");

        entity.HasOne(d => d.Cart)
            .WithMany(p => p.CartTickets)
            .HasForeignKey(d => d.CartId)
            .HasConstraintName("FK_CartTickets_Cart");

        entity.HasOne(d => d.SectionTicket)
            .WithMany(p => p.CartTickets)
            .HasForeignKey(d => d.SectionTicketId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_CartTickets_SectionTickets");

        entity.HasOne(d => d.User)
            .WithMany(p => p.CartTickets)
            .HasPrincipalKey(p => p.UserId)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("FK_CartTickets_AspNetUsers");
    }
}
