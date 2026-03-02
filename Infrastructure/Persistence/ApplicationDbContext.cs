using Domain.Common;
using Eshop.Domain.Entities.Carts;
using Eshop.Domain.Entities.Lukups;
using Eshop.Domain.Entities.Museum;
using Eshop.Domain.Entities.OrderTicket;
using Eshop.Domain.Entities.People;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;

namespace Infrastructure.Persistence;

public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, ApplicationRole, string>
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

        // Prevent EF from discovering entity types not mapped in this DbContext.
        // Ignoring navigation properties alone is not enough — EF Core still adds the target
        // entity types to the model and then generates shadow FKs or throws on type mismatches
        // (e.g. AnnualMembership.UserId is int but ApplicationUser.Id is string).
        // [Owned] types like RefreshToken also require model-level Ignore to be fully excluded.
        builder.Ignore<Sections>();
        builder.Ignore<UsersTitles>();
        builder.Ignore<AnnualMembership>();
        builder.Ignore<UserToRole>();
        builder.Ignore<RefreshToken>();
        builder.Ignore<SiteVisits>();
        builder.Ignore<Cart>();
        builder.Ignore<CartTickets>();
        builder.Ignore<Orders>();
        builder.Ignore<IssuedTickets>();

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property("_permissionsForUser")
                .HasColumnName("PermissionsForUser")
                .HasColumnType("nvarchar(MAX)");

            // CreatedBy/UpdatedBy are from IBaseEntity but don't exist in AspNetUsers table.
            // The DB uses the legacy CreateBy/EditBy columns instead.
            entity.Ignore(u => u.CreatedBy);
            entity.Ignore(u => u.UpdatedBy);

            // Ignore navigation properties that reference entities not mapped in this DbContext
            entity.Ignore(u => u.Cart);
            entity.Ignore(u => u.CartTickets);
            entity.Ignore(u => u.Orders);
            entity.Ignore(u => u.IssuedTickets);
            entity.Ignore(u => u.UserToRoles);
            entity.Ignore(u => u.SiteVisits);
            entity.Ignore(u => u.AnnualMembership);
            entity.Ignore(u => u.RefreshTokens);
            entity.Ignore(u => u.Section);
            entity.Ignore(u => u.UserTitle);
        });

        builder.Entity<ApplicationRole>(entity =>
        {
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
