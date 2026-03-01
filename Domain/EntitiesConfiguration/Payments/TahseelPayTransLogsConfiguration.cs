
namespace Eshop.Domain.EntitiesConfiguration.Payments;

public class TahseelPayTransLogsConfiguration : IEntityTypeConfiguration<TahseelPayTransLogs>
{
    public void Configure(EntityTypeBuilder<TahseelPayTransLogs> entity)
    {
        
        entity.Property(e => e.TahseelPayTransId).HasColumnName("TahseelPayTransID");
        entity.Property(e => e.PaymentTransId).HasColumnName("PaymentTransID");
        
        entity.HasOne(d => d.TahseelPayTrans)
             .WithMany(c => c.TahseelPayTransLogs)
             .HasForeignKey(d => d.TahseelPayTransId)
             .HasConstraintName("FK_TahseelPayTransLogs_TahseelPayTrans");

        entity.HasOne(d => d.PaymentTrans)
             .WithMany(c => c.TahseelPayTransLogs)
             .HasForeignKey(d => d.PaymentTransId)
             .HasPrincipalKey(p => p.Id)
             .HasConstraintName("FK_TahseelPayTransLogs_PaymentTrans");
    }
}
