namespace Eshop.Domain.EntitiesConfiguration.Museum;

public class SectionTicketJoinsConfiguration : IEntityTypeConfiguration<SectionTicketJoins>
{
    // represent tickets for each section "museum".
    public void Configure(EntityTypeBuilder<SectionTicketJoins> entity)
    {
        

        entity.Property(e => e.SectionTicketId).HasColumnName("SectionTicketId");
        entity.Property(e => e.SectionId).HasColumnName("SectionID");
        entity.HasOne(d => d.Section)
          .WithMany(p => p.SectionTicketJoins)
          .HasForeignKey(d => d.SectionId)
          .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(d => d.SectionTicket)
          .WithMany(p => p.SectionTicketJoins)
          .HasForeignKey(d => d.SectionTicketId)
          .OnDelete(DeleteBehavior.Restrict);
    }
}
