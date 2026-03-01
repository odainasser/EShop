namespace Eshop.Domain.EntitiesConfiguration.OrderTicket;

public class OrdersConfiguration : IEntityTypeConfiguration<Orders>
{
    public void Configure(EntityTypeBuilder<Orders> entity)
    {
        
        entity.Property(e => e.ChannelCode).HasDefaultValue(1);

        entity.Property(e => e.Email).HasMaxLength(256);

        entity.Property(e => e.FullName).HasMaxLength(256);

        entity.Property(e => e.OrderRef).HasMaxLength(500);

        entity.Property(e => e.PaymentTypeCode).HasMaxLength(250);

        entity.Property(e => e.StatusCode).HasMaxLength(250);

        entity.Property(e => e.TotalAmount)
            .HasDefaultValueSql("((0))");

        entity.HasOne(d => d.User)
            .WithMany(p => p.Orders)
            .HasPrincipalKey(p => p.UserId)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FK_Orders_AspNetUsers");

        entity.Property(o => o.OrderSeqRef)
            .HasDefaultValueSql("NEXT VALUE FOR shared.OrdersNumbers");
    }
}
