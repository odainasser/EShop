namespace Eshop.Domain.EntitiesConfiguration.Common;

public static class ConfigurationExtensions
{
    public static ModelBuilder AddApplicationSequence(this ModelBuilder modelBuilder)
    {
        modelBuilder.HasSequence<int>("OrdersNumbers", schema: "shared")
            .StartsAt(100000000)
            .IncrementsBy(1);

        modelBuilder.HasSequence<int>("TicketsNumbers", schema: "shared")
            .StartsAt(100000000)
            .IncrementsBy(1);

        modelBuilder.HasSequence<int>("PaymentTransNumbers", schema: "shared")
            .StartsAt(100000000)
            .IncrementsBy(1);
        return modelBuilder;
    }
}