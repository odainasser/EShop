namespace Eshop.Domain.EntitiesConfiguration.Carts;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> entity)
    {
        entity.Property(e => e.StatusCode).HasMaxLength(250);
        entity.HasOne(d => d.User)
            .WithMany(p => p.Cart)
            .HasPrincipalKey(p => p.UserId)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Cart_AspNetUsers");
    }
}
