using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.UserLogs;
using Application.Services;
using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Services;

public class UserLogService : IUserLogService
{
    private readonly IUserLogRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public UserLogService(IUserLogRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task LogAsync(CreateUserLogRequest request)
    {
        var userId = request.UserId;
        var userName = request.UserName;

        // If UserId is not provided, get from current user
        if (userId == Guid.Empty || string.IsNullOrEmpty(userName))
        {
            try
            {
                var (currentUserId, currentUserName) = await _currentUserService.GetCurrentUserAsync();
                if (userId == Guid.Empty)
                {
                    userId = currentUserId;
                }
                if (string.IsNullOrEmpty(userName))
                {
                    userName = currentUserName;
                }
            }
            catch
            {
                // If we can't get current user, continue with empty values
            }
        }

        var log = new UserLog
        {
            UserId = userId,
            UserName = userName ?? "Unknown",
            Action = request.Action,
            EntityName = request.EntityName,
            EntityId = request.EntityId,
            Timestamp = DateTime.UtcNow
        };

        await _repository.AddAsync(log);
        await _repository.SaveChangesAsync();
    }

    public async Task<PaginatedList<UserLogDto>> GetLogsAsync(int pageNumber, int pageSize, Guid? userId = null, string? entityName = null, string? entityId = null)
    {
        var (items, totalCount) = await _repository.GetPagedLogsAsync(pageNumber, pageSize, userId, entityName, entityId);

        var dtos = items.Select(l => new UserLogDto
        {
            Id = l.Id,
            UserId = l.UserId,
            UserName = l.UserName,
            Action = l.Action.ToString(),
            EntityName = l.EntityName,
            EntityId = l.EntityId,
            Timestamp = l.Timestamp
        }).ToList();

        return new PaginatedList<UserLogDto>(dtos, totalCount, pageNumber, pageSize);
    }
}
