namespace Eshop.Domain.EntitiesConfiguration.Common;

public static class SetBaseEntityConfiguration
{
    public static void ConfigureBaseEntity<T>(this EntityTypeBuilder<T> configuration) where T : BaseEntity
    {
        configuration.Property(e => e.CreateAt)
            .HasDefaultValueSql("(getdate())");

        configuration.Property(o => o.IsActive)
            .HasDefaultValue(true)
            .HasSentinel(false);

        configuration.Property(o => o.IsSynced)
            .HasDefaultValue(false)
            .HasSentinel(true);
    }
}