
namespace Eshop.Domain.EntitiesConfiguration.People;

public class UsersTitlesConfiguration : IEntityTypeConfiguration<UsersTitles>
{
    public void Configure(EntityTypeBuilder<UsersTitles> entity)
    {
        
        entity.HasKey(g => new { g.Id });
        entity.HasIndex(e => e.Code).IsUnique();
    }
}

public class UsersTitleTranslationsConfiguration : IEntityTypeConfiguration<UsersTitleTranslations>
{
    public void Configure(EntityTypeBuilder<UsersTitleTranslations> entity)
    {
        

        entity.Property(e => e.Title).HasMaxLength(500);
        entity.Property(e => e.Desc).HasMaxLength(500);

        entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

        entity.Property(e => e.UsersTitleId).HasColumnName("UsersTitleID");

        entity.HasOne(d => d.Language)
            .WithMany(p => p.UsersTitleTranslations)
            .HasForeignKey(d => d.LanguageId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_UsersTitleTranslations_Languages");

        entity.HasOne(d => d.UsersTitle)
            .WithMany(p => p.UsersTitleTranslations)
            .HasForeignKey(d => d.UsersTitleId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_UsersTitleTranslations_UsersTitles");
    }
}