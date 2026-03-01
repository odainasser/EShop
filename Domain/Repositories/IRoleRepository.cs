namespace Domain.Repositories;

public interface IRoleRepository : IRepository<RoleToPermissions>
{
    Task<RoleToPermissions?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<RoleToPermissions>> GetRolesByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<RoleToPermissions?> GetRoleWithPermissionsAsync(int roleId, CancellationToken cancellationToken = default);
}
