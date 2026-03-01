using Web.Models;
using Web.Models.Users;

namespace Web.Services;

public interface IUserManagementService
{
    Task<PaginatedList<UserResponse>> GetAllUsersAsync(int pageNumber, int pageSize, string? role = null, string? excludeRole = null);
    Task<UserResponse?> GetUserByIdAsync(Guid userId);
    Task<UserResponse> CreateUserAsync(CreateUserRequest request);
    Task<UserResponse> UpdateUserAsync(Guid userId, UpdateUserRequest request);
    Task<UserResponse> UpdateProfileAsync(UpdateUserRequest request);
    Task DeleteUserAsync(Guid userId);
    Task<UserResponse> AssignRolesToUserAsync(Guid userId, AssignRolesRequest request);
    Task<bool> CheckEmailExistsAsync(string email);
}
