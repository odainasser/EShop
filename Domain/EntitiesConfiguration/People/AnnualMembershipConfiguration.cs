

namespace Eshop.Domain.EntitiesConfiguration.People;

public class AnnualMembershipConfiguration : IEntityTypeConfiguration<AnnualMembership>
{
    public void Configure(EntityTypeBuilder<AnnualMembership> entity)
    {
        entity.ToTable("AnnualMembership");      // matches [dbo].[AnnualMembership]
        entity.HasKey(e => e.Id);                // from BaseEntity.Id

        // Column mappings (lengths from your script)
        entity.Property(e => e.FirstName)
              .HasMaxLength(50);

        entity.Property(e => e.LastName)
              .HasMaxLength(50);

        entity.Property(e => e.Email)
              .HasMaxLength(256);

        entity.Property(e => e.MembershipNumber)
              .HasMaxLength(256);

        entity.Property(e => e.OrderReference)
              .HasMaxLength(256);

        entity.HasOne(d => d.User)
         .WithMany(p => p.AnnualMembership)
         .HasPrincipalKey(p => p.UserId)
         .HasForeignKey(d => d.UserId)
         .OnDelete(DeleteBehavior.Cascade)
         .HasConstraintName("FK_AnnualMembership_AspNetUsers_UserId");
    }
}

