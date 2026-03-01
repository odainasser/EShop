
namespace Eshop.Domain.EntitiesConfiguration.Lukups;

public class AppModuleLookupsConfiguration : IEntityTypeConfiguration<AppModuleLookups>
{
    public void Configure(EntityTypeBuilder<AppModuleLookups> entity)
    {
        entity.HasIndex(e => e.Code)
               .HasName("ICode_AppModuleLookups");

        entity.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(250);

        entity.Property(e => e.DescAr).HasMaxLength(500);

        entity.Property(e => e.DescEn).HasMaxLength(500);

        entity.Property(e => e.NameAr).HasMaxLength(500);

        entity.Property(e => e.NameEn).HasMaxLength(500);

        entity.HasOne(d => d.AppLookupType)
            .WithMany(p => p.AppModuleLookups)
            .HasForeignKey(d => d.AppLookupTypeCode)
            .HasPrincipalKey(lt => lt.Code)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_AppModuleLookups_AppLookupTypes");

        entity.HasOne(d => d.AppModule)
            .WithMany(p => p.AppModuleLookups)
            .HasForeignKey(d => d.AppModuleCode)
            .HasPrincipalKey(m => m.Code)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_AppModuleLookups_AppModules");
    }
}
