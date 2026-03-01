using Domain.Entities;
using Domain.Common;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Domain.Enums;

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

    public DbSet<User> DomainUsers { get; set; }
    public DbSet<Role> DomainRoles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<Lookup> Lookups { get; set; }
    public DbSet<SystemSetting> SystemSettings { get; set; }
    public DbSet<UserLog> UserLogs { get; set; }
    public DbSet<Media> Media { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure ApplicationRole
        builder.Entity<ApplicationRole>(entity =>
        {
            entity.Property(r => r.DescriptionEn).HasMaxLength(500);
            entity.Property(r => r.DescriptionAr).HasMaxLength(500);
            entity.Property(r => r.IsSystemRole).IsRequired();
        });

        // Configure Lookup entity
        builder.Entity<Lookup>(entity =>
        {
            entity.ToTable("Lookups");
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Code).IsRequired().HasMaxLength(50);
            entity.Property(l => l.NameEn).IsRequired().HasMaxLength(200);
            entity.Property(l => l.NameAr).IsRequired().HasMaxLength(200).IsUnicode(true);
            // store enum as string in the database
            entity.Property(l => l.Type)
                .IsRequired()
                .HasMaxLength(50)
                .HasConversion<string>();

            entity.HasOne(l => l.Parent)
                .WithMany(l => l.Children)
                .HasForeignKey(l => l.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(l => !l.IsDeleted);
        });

        // Configure Domain User entity
        builder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.FirstName).HasMaxLength(100);
            entity.Property(u => u.LastName).HasMaxLength(100);

            entity.HasQueryFilter(u => !u.IsDeleted);
        });

        // Configure Domain Role entity
        builder.Entity<Role>(entity =>
        {
            entity.ToTable("Roles");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired().HasMaxLength(256);
            entity.HasIndex(r => r.Name).IsUnique();
        });

        // Configure Permission entity
        builder.Entity<Permission>(entity =>
        {
            entity.ToTable("Permissions");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Code).IsRequired().HasMaxLength(100);
            entity.HasIndex(p => p.Code).IsUnique();
            entity.Property(p => p.NameEn).IsRequired().HasMaxLength(256);
            entity.Property(p => p.NameAr).IsRequired().HasMaxLength(256);
            entity.Property(p => p.DescriptionEn).HasMaxLength(500);
            entity.Property(p => p.DescriptionAr).HasMaxLength(500);
        });

        // Configure UserRole many-to-many
        builder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRoles");
            entity.HasKey(ur => new { ur.UserId, ur.RoleId });

            entity.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure RolePermission many-to-many
        builder.Entity<RolePermission>(entity =>
        {
            entity.ToTable("RolePermissions");
            entity.HasKey(rp => new { rp.RoleId, rp.PermissionId });

            entity.HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure SystemSetting entity
        builder.Entity<SystemSetting>(entity =>
        {
            entity.ToTable("SystemSettings");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Key).IsUnique();
            entity.Property(e => e.Key).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Value).IsRequired();
            entity.Property(e => e.Group).IsRequired().HasMaxLength(50);

            entity.HasQueryFilter(s => !s.IsDeleted);
        });

        // Configure UserLog entity
        builder.Entity<UserLog>(entity =>
        {
            entity.ToTable("UserLogs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Action)
                .IsRequired()
                .HasMaxLength(50)
                .HasConversion<string>();
            entity.Property(e => e.EntityName).HasMaxLength(100);
            entity.Property(e => e.EntityId).HasMaxLength(100);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Timestamp);
        });

        // Configure Media entity
        builder.Entity<Media>(entity =>
        {
            entity.ToTable("Media");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.EntityType).IsRequired().HasMaxLength(100);
            entity.Property(m => m.EntityId).IsRequired();
            entity.Property(m => m.CollectionName).IsRequired().HasMaxLength(100);
            entity.Property(m => m.Name).IsRequired().HasMaxLength(255);
            entity.Property(m => m.FileName).IsRequired().HasMaxLength(255);
            entity.Property(m => m.MimeType).HasMaxLength(100);
            entity.Property(m => m.Disk).HasMaxLength(50);
            entity.Property(m => m.Path).IsRequired().HasMaxLength(500);
            
            entity.HasIndex(m => new { m.EntityType, m.EntityId });
            entity.HasQueryFilter(m => !m.IsDeleted);
        });

        // Museum entity removed during cleanup.
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