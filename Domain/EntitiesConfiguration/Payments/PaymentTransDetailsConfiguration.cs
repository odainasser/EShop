
namespace Eshop.Domain.EntitiesConfiguration.Payments;

public class PaymentTransDetailsConfiguration : IEntityTypeConfiguration<PaymentTransDetails>
{
    public void Configure(EntityTypeBuilder<PaymentTransDetails> entity)
    {
        
        entity.Property(e => e.PaymentTransId).HasColumnName("PaymentTransID");
        entity.Property(e => e.ItemReferenceId).HasColumnName("ItemReferenceID");
        entity.Property(e => e.ItemType).HasDefaultValue("T");

        entity.Property(e => e.TotalVat)
            .HasColumnName("TotalVAT");

        entity.HasOne(d => d.PaymentTrans)
            .WithMany(p => p.PaymentTransDetails)
            .HasForeignKey(d => d.PaymentTransId)
            .HasConstraintName("FK_PaymentTransDetails_PaymentTrans");
    }
}
