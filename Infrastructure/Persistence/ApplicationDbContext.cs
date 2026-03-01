using Domain.Common;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Application.Common.Interfaces;

namespace Infrastructure.Persistence;

public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    private readonly ICurrentUserService? _currentUserService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(u => u.Id)
                .HasConversion(id => id.ToString(), str => Guid.Parse(str));
            entity.Ignore(u => u.IsSystemUser);
            entity.Ignore(u => u.CreatedAt);
            entity.Ignore(u => u.UpdatedAt);
        });

        builder.Entity<ApplicationRole>(entity =>
        {
            entity.Property(r => r.Id)
                .HasConversion(id => id.ToString(), str => Guid.Parse(str));
            entity.Property(r => r.DescriptionEn).HasMaxLength(500);
            entity.Property(r => r.DescriptionAr).HasMaxLength(500);
            entity.Property(r => r.IsSystemRole).IsRequired();
            entity.Ignore(r => r.CreatedAt);
            entity.Ignore(r => r.UpdatedAt);
        });
    }

            public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        string? currentUserName = null;
        
        if (_currentUserService != null)
        {
            try
            {
                var result = await _currentUserService.GetCurrentUserAsync();
                if (result.Item1 != Guid.Empty)
                {
                    currentUserName = result.Item2;
                }
            }
            catch
            {
                // If we can't get current user, continue without it
            }
        }

        UpdateAuditFields(currentUserName);

        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        string? currentUserName = null;
        
        if (_currentUserService != null)
        {
            try
            {
                var task = _currentUserService.GetCurrentUserAsync();
                task.Wait();
                var result = task.Result;
                if (result.Item1 != Guid.Empty)
                {
                    currentUserName = result.Item2;
                }
            }
            catch
            {
                // If we can't get current user, continue without it
            }
        }

        UpdateAuditFields(currentUserName);

        return base.SaveChanges();
    }

    private void UpdateAuditFields(string? currentUserName)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    if (!string.IsNullOrEmpty(currentUserName))
                    {
                        entry.Entity.CreatedBy = currentUserName;
                    }
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    if (!string.IsNullOrEmpty(currentUserName))
                    {
                        entry.Entity.UpdatedBy = currentUserName;
                    }
                    break;

                case EntityState.Deleted:
                    if (entry.Entity is BaseAuditableEntity auditable)
                    {
                        entry.State = EntityState.Modified;
                        auditable.IsDeleted = true;
                        auditable.DeletedAt = DateTime.UtcNow;
                        if (!string.IsNullOrEmpty(currentUserName))
                        {
                            auditable.DeletedBy = currentUserName;
                        }
                    }
                    break;
            }
        }
    }
}