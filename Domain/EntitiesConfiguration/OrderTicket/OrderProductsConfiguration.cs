
namespace Eshop.Domain.EntitiesConfiguration.OrderTicket;

public class OrderProductsConfiguration : IEntityTypeConfiguration<OrderProducts>
{
    public void Configure(EntityTypeBuilder<OrderProducts> entity)
    {
        {
            entity.Property(e => e.Discount)
                .HasDefaultValueSql("((0))");

            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Orders_OrderProducts");
        }
    }

}