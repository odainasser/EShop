
namespace Eshop.Domain.EntitiesConfiguration.Payments;

public class PaymentTransConfiguration : IEntityTypeConfiguration<PaymentTrans>
{
    public void Configure(EntityTypeBuilder<PaymentTrans> entity)
    {
        
        entity.Property(e => e.UserId).HasColumnName("UserID");
        entity.Property(e => e.OrderId).HasColumnName("OrderID");
        
        entity.Property(e => e.ChannelCode).HasDefaultValue(1);

        entity.Property(e => e.ErrorMessage).HasMaxLength(1000);

        entity.Property(e => e.StatusCode).HasMaxLength(250);
        entity.Property(e => e.TotalVat)
            .HasColumnName("TotalVAT");

        entity.HasOne(d => d.Order)
            .WithMany(p => p.PaymentTrans)
            .HasForeignKey(d => d.OrderId)
            .HasConstraintName("FK_Orders_PaymentTrans");

      
        entity
           .Property(o => o.PaySeqRef)
           .HasDefaultValueSql("NEXT VALUE FOR shared.PaymentTransNumbers");
    }
}
