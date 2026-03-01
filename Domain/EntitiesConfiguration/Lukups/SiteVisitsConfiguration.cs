
namespace Eshop.Domain.EntitiesConfiguration.Lukups;

public class SiteVisitsConfiguration : IEntityTypeConfiguration<SiteVisits>
{
    public void Configure(EntityTypeBuilder<SiteVisits> entity)
    {
        entity.Property(e => e.UserId).HasColumnName("UserID");
        entity.HasOne(d => d.User)
            .WithMany(p => p.SiteVisits)
            .HasForeignKey(d => d.UserId)
            .HasPrincipalKey(e => e.UserId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}
