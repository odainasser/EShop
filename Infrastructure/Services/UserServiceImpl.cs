using Application.Common.Models;
using Application.Features.Users;
using Application.Services;
using Domain.Exceptions;
using Eshop.Domain.Entities.People;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Application.Common.Interfaces;
using Application.Common.Behaviors;
using FluentValidation.Results;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ICurrentUserService _currentUserService;

    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedList<UserResponse>> GetAllUsersAsync(int pageNumber, int pageSize, string? role = null, string? excludeRole = null, CancellationToken cancellationToken = default)
    {
        // Filter by specific role
        if (!string.IsNullOrEmpty(role))
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(role);
            var roleCount = usersInRole.Count;
            var pagedRoleUsers = usersInRole.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            
            var roleUserResponses = new List<UserResponse>();
            foreach (var user in pagedRoleUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                roleUserResponses.Add(await MapToUserResponseAsync(user, roles.ToList()));
            }
            
            return new PaginatedList<UserResponse>(roleUserResponses, roleCount, pageNumber, pageSize);
        }

        // Exclude users with specific role
        if (!string.IsNullOrEmpty(excludeRole))
        {
            var usersInExcludedRole = await _userManager.GetUsersInRoleAsync(excludeRole);
            var excludedUserIds = usersInExcludedRole.Select(u => u.Id).ToHashSet();
            
            var query = _userManager.Users.Where(u => !excludedUserIds.Contains(u.Id));
            var allFilteredUsers = await query.ToListAsync(cancellationToken);
            var count = allFilteredUsers.Count;
            var users = allFilteredUsers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            
            var userResponses = new List<UserResponse>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userResponses.Add(await MapToUserResponseAsync(user, roles.ToList()));
            }
            
            return new PaginatedList<UserResponse>(userResponses, count, pageNumber, pageSize);
        }

        // Default: get all users
        var defaultQuery = _userManager.Users;
        var defaultCount = await defaultQuery.CountAsync(cancellationToken);
        var defaultUsers = await defaultQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        
        var defaultUserResponses = new List<UserResponse>();
        foreach (var user in defaultUsers)
        {
            var roles = await _userManager.GetRolesAsync(user);
            defaultUserResponses.Add(await MapToUserResponseAsync(user, roles.ToList()));
        }
        
        return new PaginatedList<UserResponse>(defaultUserResponses, defaultCount, pageNumber, pageSize);
    }

    public async Task<UserResponse?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);
        return await MapToUserResponseAsync(user, roles.ToList());
    }

    public async Task<UserResponse> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new UserAlreadyExistsException(request.Email);
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            IsActive = request.IsActive,
            EmailConfirmed = false
        };

        // Validate password presence for admin-created users: require password and confirm match
        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new InvalidOperationException("Password is required when creating a user from the admin UI.");
        }

        if (request.Password != request.ConfirmPassword)
        {
            throw new InvalidOperationException("Passwords do not match.");
        }

        var result = await _userManager.CreateAsync(user, request.Password);
        
        if (!result.Succeeded)
        {
            var failures = result.Errors.Select(e => MapIdentityErrorToValidationFailure(e));
            throw new ValidationException(failures);
        }


        if (request.RoleId.HasValue)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId.Value.ToString());
            if (role != null)
            {
                // Prevent creating users with the Client role from the admin UI.
                // Clients must register through the registration form.
                if (string.Equals(role.Name, "Client", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("Cannot assign Client role from admin panel. Clients must register through the registration form.");
                }
                await _userManager.AddToRoleAsync(user, role.Name!);
            }
        }

        var roles = await _userManager.GetRolesAsync(user);
        return await MapToUserResponseAsync(user, roles.ToList());
    }

    public async Task<UserResponse> UpdateUserAsync(Guid userId, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new UserNotFoundException($"User with ID '{userId}' not found.");
        }

        // Get current user to check if they're updating themselves
        var (currentUserId, _) = await _currentUserService.GetCurrentUserAsync();
        var isSelfUpdate = currentUserId == userId;

        // Prevent modifying system users by other users (but allow self-update)
        if (user.IsAdmin && !isSelfUpdate)
        {
            throw new SystemUserModificationException();
        }

        if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new UserAlreadyExistsException(request.Email);
            }
            user.Email = request.Email;
            user.UserName = request.Email;
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        user.IsActive = request.IsActive;
        user.EmailConfirmed = request.EmailConfirmed;
        user.PhoneNumberConfirmed = request.PhoneNumberConfirmed;
        user.UpdatedAt = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        var roles = await _userManager.GetRolesAsync(user);
        return await MapToUserResponseAsync(user, roles.ToList());
    }

    public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new UserNotFoundException($"User with ID '{userId}' not found.");
        }

        // Prevent deleting system users
        if (user.IsAdmin)
        {
            throw new SystemUserModificationException();
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }

    public async Task<UserResponse> AssignRolesToUserAsync(Guid userId, AssignRolesRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new UserNotFoundException($"User with ID '{userId}' not found.");
        }

        // Prevent changing roles for system users who are Administrators
        if (user.IsAdmin && await _userManager.IsInRoleAsync(user, "Administrator"))
        {
            throw new SystemUserModificationException();
        }

        // Prevent changing roles for users who have the Client role
        if (await _userManager.IsInRoleAsync(user, "Client"))
        {
            throw new InvalidOperationException("Cannot change the role of a client user.");
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Any())
        {
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
        }

        if (request.RoleId.HasValue)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId.Value.ToString());
            if (role != null)
            {
                // Prevent assigning Client role from admin panel - clients are created via registration only
                if (string.Equals(role.Name, "Client", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("Cannot assign Client role from admin panel. Clients must register through the registration form.");
                }
                await _userManager.AddToRoleAsync(user, role.Name!);
            }
        }

        var roles = await _userManager.GetRolesAsync(user);
        return await MapToUserResponseAsync(user, roles.ToList());
    }

    public async Task<IEnumerable<UserResponse>> GetActiveUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userManager.Users.Where(u => u.IsActive).ToListAsync(cancellationToken);
        
        var userResponses = new List<UserResponse>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userResponses.Add(await MapToUserResponseAsync(user, roles.ToList()));
        }
        
        return userResponses;
    }

    private async Task<UserResponse> MapToUserResponseAsync(ApplicationUser user, List<string> roleNames)
    {
        var roleDtos = new List<RoleDto>();
        
        foreach (var roleName in roleNames)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                roleDtos.Add(new RoleDto
                {
                    Id = Guid.Parse(role.Id.ToString()),
                    Name = role.Name!,
                    NameEn = role.Name!,
                    NameAr = role.NameAr,
                    DescriptionEn = role.DescriptionEn,
                    DescriptionAr = role.DescriptionAr
                });
            }
        }

        string? avatarUrl = null;

        return new UserResponse
        {
            Id = Guid.Parse(user.Id.ToString()),
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = $"{user.FirstName} {user.LastName}",
            PhoneNumber = user.PhoneNumber,
            IsActive = user.IsActive,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
            TwoFactorEnabled = user.TwoFactorEnabled,
            LockoutEnabled = user.LockoutEnabled,
            LockoutEnd = user.LockoutEnd?.DateTime,
            AccessFailedCount = user.AccessFailedCount,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            Roles = roleDtos,
            IsSystemUser = user.IsAdmin,
            AvatarUrl = avatarUrl
        };
    }

    private static string GetDisplayName(ApplicationUser user)
    {
        var first = user.FirstName?.Trim();
        var last = user.LastName?.Trim();
        var full = string.IsNullOrEmpty(first) && string.IsNullOrEmpty(last) ? null : $"{first} {last}".Trim();
        return string.IsNullOrEmpty(full) ? user.Email ?? "Unknown" : full;
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _userManager.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }

    private static ValidationFailure MapIdentityErrorToValidationFailure(IdentityError error)
    {
        string propertyName = string.Empty;
        
        if (error.Code.StartsWith("Password"))
        {
            propertyName = "Password";
        }
        else if (error.Code.Contains("Email") || error.Code.Contains("UserName"))
        {
            propertyName = "Email";
        }
        
        // Use Code as the Error Message key for localization
        return new ValidationFailure(propertyName, error.Code);
    }
}
