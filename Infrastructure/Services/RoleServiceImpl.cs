using Application.Common.Models;
using Application.Features.Roles;
using Application.Services;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Application.Features.UserLogs;
using Domain.Enums;
using Application.Common.Interfaces;

namespace Infrastructure.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IUserLogService _userLogService;
    private readonly ICurrentUserService _currentUserService;

    public RoleService(
        RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context,
        IUserLogService userLogService,
        ICurrentUserService currentUserService)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _context = context;
        _userLogService = userLogService;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedList<RoleResponse>> GetAllRolesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _roleManager.Roles;
        var count = await query.CountAsync(cancellationToken);
        var roles = await _roleManager.Roles
            .OrderBy(r => r.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var roleResponses = new List<RoleResponse>();
        
        foreach (var role in roles)
        {
            var userCount = (await _userManager.GetUsersInRoleAsync(role.Name!)).Count;
            // Get permission count from RolePermissions table
            var permissionCount = await _context.RolePermissions
                .Where(rp => rp.RoleId == role.Id)
                .CountAsync(cancellationToken);

            roleResponses.Add(new RoleResponse
            {
                Id = Guid.Parse(role.Id.ToString()),
                NameEn = role.Name!,
                NameAr = role.NameAr,
                DescriptionEn = role.DescriptionEn,
                DescriptionAr = role.DescriptionAr,
                IsSystemRole = role.IsSystemRole,
                IsActive = role.IsActive,
                CreatedAt = role.CreatedAt,
                UpdatedAt = role.UpdatedAt,
                UserCount = userCount,
                PermissionCount = permissionCount
            });
        }

        return new PaginatedList<RoleResponse>(roleResponses, count, pageNumber, pageSize);
    }

    public async Task<RoleDetailResponse?> GetRoleByIdAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        if (role == null)
        {
            return null;
        }

        var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
        
        // Load permissions
        var domainRole = await _context.DomainRoles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);
            
        var permissions = domainRole?.RolePermissions
            .Select(rp => new PermissionDto
            {
                Id = rp.Permission.Id,
                Code = rp.Permission.Code,
                Name = rp.Permission.NameEn,
                NameEn = rp.Permission.NameEn,
                NameAr = rp.Permission.NameAr,
                Description = rp.Permission.DescriptionEn,
                DescriptionEn = rp.Permission.DescriptionEn,
                DescriptionAr = rp.Permission.DescriptionAr,
                Module = rp.Permission.Module
            })
            .ToList() ?? new List<PermissionDto>();

        return new RoleDetailResponse
        {
            Id = Guid.Parse(role.Id.ToString()),
            NameEn = role.Name!,
            NameAr = role.NameAr,
            DescriptionEn = role.DescriptionEn,
            DescriptionAr = role.DescriptionAr,
            IsSystemRole = role.IsSystemRole,
            CreatedAt = role.CreatedAt,
            UpdatedAt = role.UpdatedAt,
            UserCount = usersInRole.Count,
            PermissionCount = permissions.Count,
            Permissions = permissions,
            IsActive = role.IsActive
        };
    }

    public async Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken = default)
    {
        var existingRole = await _roleManager.FindByNameAsync(request.NameEn);
        if (existingRole != null)
        {
            throw new RoleAlreadyExistsException(request.NameEn);
        }

        var role = new ApplicationRole
        {
            Name = request.NameEn,
            NameAr = request.NameAr,
            DescriptionEn = request.DescriptionEn,
            DescriptionAr = request.DescriptionAr,
            IsSystemRole = request.IsSystemRole,
            IsActive = request.IsActive
        };

        var result = await _roleManager.CreateAsync(role);
        
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        var (currentUserId, currentUserName) = await _currentUserService.GetCurrentUserAsync();
        if (currentUserId != Guid.Empty)
        {
            await _userLogService.LogAsync(new CreateUserLogRequest
            {
                UserId = currentUserId,
                UserName = currentUserName,
                Action = AuditAction.Created,
                EntityName = "Role",
                EntityId = role.Id.ToString()
            });
        }

        return new RoleResponse
        {
            Id = Guid.Parse(role.Id.ToString()),
            NameEn = role.Name!,
            NameAr = role.NameAr,
            DescriptionEn = role.DescriptionEn,
            DescriptionAr = role.DescriptionAr,
            IsSystemRole = role.IsSystemRole,
            CreatedAt = role.CreatedAt,
            UpdatedAt = role.UpdatedAt,
            UserCount = 0,
            PermissionCount = 0,
            IsActive = role.IsActive
        };
    }

    public async Task<RoleResponse> UpdateRoleAsync(Guid roleId, UpdateRoleRequest request, CancellationToken cancellationToken = default)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        if (role == null)
        {
            throw new RoleNotFoundException(roleId);
        }

        if (role.IsSystemRole)
        {
            throw new SystemRoleModificationException();
        }

        if (role.Name != request.NameEn)
        {
            var existingRole = await _roleManager.FindByNameAsync(request.NameEn);
            if (existingRole != null)
            {
                throw new RoleAlreadyExistsException(request.NameEn);
            }
        }

        role.Name = request.NameEn;
        role.NameAr = request.NameAr;
        role.DescriptionEn = request.DescriptionEn;
        role.DescriptionAr = request.DescriptionAr;
        role.IsActive = request.IsActive;
        role.UpdatedAt = DateTime.UtcNow;

        var result = await _roleManager.UpdateAsync(role);
        
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        var (currentUserId, currentUserName) = await _currentUserService.GetCurrentUserAsync();
        if (currentUserId != Guid.Empty)
        {
            await _userLogService.LogAsync(new CreateUserLogRequest
            {
                UserId = currentUserId,
                UserName = currentUserName,
                Action = AuditAction.Updated,
                EntityName = "Role",
                EntityId = role.Id.ToString()
            });
        }

        var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);

        return new RoleResponse
        {
            Id = Guid.Parse(role.Id.ToString()),
            NameEn = role.Name!,
            NameAr = role.NameAr,
            DescriptionEn = role.DescriptionEn,
            DescriptionAr = role.DescriptionAr,
            IsSystemRole = role.IsSystemRole,
            CreatedAt = role.CreatedAt,
            UpdatedAt = role.UpdatedAt,
            UserCount = usersInRole.Count,
            PermissionCount = 0,
            IsActive = role.IsActive
        };
    }

    public async Task DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        if (role == null)
        {
            throw new RoleNotFoundException(roleId);
        }

        if (role.IsSystemRole)
        {
            throw new SystemRoleModificationException();
        }

        // Prevent deleting role if there are assigned users
        var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
        if (usersInRole != null && usersInRole.Any())
        {
            // Throw exception with role name so UI shows the name instead of the GUID
            throw new RoleAssignedException(role.Name!);
        }

        var result = await _roleManager.DeleteAsync(role);
        
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        var (currentUserId, currentUserName) = await _currentUserService.GetCurrentUserAsync();
        if (currentUserId != Guid.Empty)
        {
            await _userLogService.LogAsync(new CreateUserLogRequest
            {
                UserId = currentUserId,
                UserName = currentUserName,
                Action = AuditAction.Deleted,
                EntityName = "Role",
                EntityId = role.Id.ToString()
            });
        }
    }

    public async Task<RoleDetailResponse> AssignPermissionsToRoleAsync(Guid roleId, AssignPermissionsRequest request, CancellationToken cancellationToken = default)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        if (role == null)
        {
            throw new RoleNotFoundException(roleId);
        }

        // Get domain role by Id first
        var domainRole = await _context.DomainRoles
            .Include(r => r.RolePermissions)
            .FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);

        if (domainRole == null)
        {
            // Check if domain role exists by Name (in case of Id mismatch)
            domainRole = await _context.DomainRoles
                .Include(r => r.RolePermissions)
                .FirstOrDefaultAsync(r => r.Name == role.Name, cancellationToken);
            
            if (domainRole != null)
            {
                // Update the existing domain role's Id to match the Identity role
                // This handles cases where there's an Id mismatch
                domainRole.Id = roleId;
                domainRole.DescriptionEn = role.DescriptionEn;
                domainRole.DescriptionAr = role.DescriptionAr;
                domainRole.IsSystemRole = role.IsSystemRole;
            }
            else
            {
                // Create domain role if it doesn't exist at all
                domainRole = new Role
                {
                    Id = roleId,
                    Name = role.Name!,
                    DescriptionEn = role.DescriptionEn,
                    DescriptionAr = role.DescriptionAr,
                    IsSystemRole = role.IsSystemRole,
                    CreatedAt = DateTime.UtcNow
                };
                _context.DomainRoles.Add(domainRole);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        // Remove existing permissions
        var existingPermissions = await _context.RolePermissions
            .Where(rp => rp.RoleId == domainRole.Id)
            .ToListAsync(cancellationToken);

        _context.RolePermissions.RemoveRange(existingPermissions);

        // Add new permissions
        foreach (var permissionId in request.PermissionIds)
        {
            var rolePermission = new RolePermission
            {
                RoleId = domainRole.Id,
                PermissionId = permissionId,
                GrantedAt = DateTime.UtcNow,
                GrantedBy = "System"
            };

            _context.RolePermissions.Add(rolePermission);
        }

        await _context.SaveChangesAsync(cancellationToken);

        var (currentUserId, currentUserName) = await _currentUserService.GetCurrentUserAsync();
        if (currentUserId != Guid.Empty)
        {
            await _userLogService.LogAsync(new CreateUserLogRequest
            {
                UserId = currentUserId,
                UserName = currentUserName,
                Action = AuditAction.Updated,
                EntityName = "Role",
                EntityId = role.Id.ToString()
            });
        }

        // Return updated role details
        return await GetRoleByIdAsync(roleId, cancellationToken) 
            ?? throw new RoleNotFoundException(roleId);
    }

    public async Task<bool> CheckRoleNameExistsAsync(string name, Guid? excludeRoleId = null, CancellationToken cancellationToken = default)
    {
        var existingRole = await _roleManager.FindByNameAsync(name);
        if (existingRole == null)
        {
            return false;
        }

        // If excludeRoleId is provided (edit mode), check if it's the same role
        if (excludeRoleId.HasValue && existingRole.Id == excludeRoleId.Value)
        {
            return false;
        }

        return true;
    }
}
