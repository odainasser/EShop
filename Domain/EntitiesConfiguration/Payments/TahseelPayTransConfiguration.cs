
namespace Eshop.Domain.EntitiesConfiguration.Payments;

public class TahseelPayTransConfiguration : IEntityTypeConfiguration<TahseelPayTrans>
{
    public void Configure(EntityTypeBuilder<TahseelPayTrans> entity)
    {
        
        entity.Property(e => e.PaymentTransId).HasColumnName("PaymentTransID");
        entity.Property(e => e.TP_PayMethod);

        entity.HasOne(d => d.PaymentTrans)
            .WithOne(c => c.TahseelPayTrans)
            .HasForeignKey<TahseelPayTrans>(d => d.PaymentTransId)
            .HasConstraintName("FK_TahseelPayTrans_PaymentTrans");
    }
}
