
namespace Eshop.Domain.EntitiesConfiguration.Museum;

public class MuseumAvailableDayTimesConfiguration : IEntityTypeConfiguration<MuseumAvailableDayTimes>
{
    public void Configure(EntityTypeBuilder<MuseumAvailableDayTimes> builder)
    {// For only getting date by Sotred Procedure.
        builder.HasNoKey().ToView(null);
    }
}
