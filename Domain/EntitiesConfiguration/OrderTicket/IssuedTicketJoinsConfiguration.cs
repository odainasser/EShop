
namespace Eshop.Domain.EntitiesConfiguration.OrderTicket;

public class IssuedTicketJoinsConfiguration : IEntityTypeConfiguration<IssuedTicketJoins>
{
    public void Configure(EntityTypeBuilder<IssuedTicketJoins> entity)
    {
        
        entity.Property(e => e.IssuedTicketId).HasColumnName("IssuedTicketID");
        entity.Property(e => e.SectionId).HasColumnName("SectionID");
        
        entity.Property(e => e.StatusCode)
            .IsRequired()
            .HasMaxLength(250)
            .HasDefaultValue("0");

        entity.HasOne(d => d.Section)
            .WithMany(p => p.IssuedTicketJoins)
            .HasForeignKey(d => d.SectionId);

        entity.HasOne(d => d.IssuedTicket)
            .WithMany(p => p.IssuedTicketJoins)
            .HasForeignKey(d => d.IssuedTicketId);
    }
}
