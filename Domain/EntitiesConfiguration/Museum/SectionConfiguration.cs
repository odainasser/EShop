namespace Eshop.Domain.EntitiesConfiguration.Museum;

public class SectionConfiguration : IEntityTypeConfiguration<Sections>
{
    public void Configure(EntityTypeBuilder<Sections> entity)
    {
        entity.Property(e => e.Code).IsRequired();
        entity.HasIndex(e => e.Code).IsUnique();
        entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
        entity.Property(e => e.Type)
        .HasColumnType("char(2)")
        .HasDefaultValueSql("'S'");
     
        entity.HasOne(d => d.Department)
            .WithMany(p => p.Sections)
            .HasForeignKey(d => d.DepartmentId)
            .HasPrincipalKey(e => e.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Sections_Departments");
    }
}
