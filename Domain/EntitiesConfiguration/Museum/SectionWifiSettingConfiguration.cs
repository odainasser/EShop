namespace Eshop.Domain.EntitiesConfiguration.Museum;

public class SectionWifiSettingConfiguration : IEntityTypeConfiguration<SectionWifiSetting>
{
    public void Configure(EntityTypeBuilder<SectionWifiSetting> builder)
    {
        builder
            .ToTable(nameof(SectionWifiSetting) + "s", SchemaConstant.IperaWifi);

        builder.HasOne(a => a.Section)
            .WithMany(a => a.SectionWifiSettings)
            .HasForeignKey(a => a.SectionId);
    }
}