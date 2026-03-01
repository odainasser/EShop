namespace Domain.Repositories;

public interface IUserLogRepository : IRepository<Audit>
{
    Task<IEnumerable<Audit>> GetLogsByUserIdAsync(string userId);
    Task<IEnumerable<Audit>> GetLatestLogsAsync(int count);
    Task<(IEnumerable<Audit> Items, int TotalCount)> GetPagedLogsAsync(int pageNumber, int pageSize, string? userId = null, string? entityName = null, string? entityId = null);
}
