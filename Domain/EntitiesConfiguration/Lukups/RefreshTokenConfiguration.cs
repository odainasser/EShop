
namespace Eshop.Domain.EntitiesConfiguration.Lukups;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> en)
    {
        en.HasKey(o => new { o.UserId, o.Id });
        en.Property(o => o.Id)
        .UseIdentityColumn(seed: 1, increment: 1);
    }
}
