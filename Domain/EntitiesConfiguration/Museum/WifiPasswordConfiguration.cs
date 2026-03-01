namespace Eshop.Domain.EntitiesConfiguration.Museum;

public class WifiPasswordConfiguration : IEntityTypeConfiguration<WifiPassword>
{
    public void Configure(EntityTypeBuilder<WifiPassword> en)
    {
        en.Property(o => o.HasExpiration)
            .HasDefaultValue(false)
            .HasSentinel(true);
    }
}
