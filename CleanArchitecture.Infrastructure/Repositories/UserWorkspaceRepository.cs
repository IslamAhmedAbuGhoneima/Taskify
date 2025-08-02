using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Domain.Models;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories;

public class UserWorkspaceRepository : BaseRepository<UserWorkspace>, IUserWorkspaceRepository
{
    readonly TaskifyDbContext _context;

    public UserWorkspaceRepository(TaskifyDbContext context)
        : base(context) => _context = context;

    public UserWorkspace? GetUserWorkspace(Guid workspaceId, string userId)
        => _context.UserWorkspaces
        .FirstOrDefault(userWorkspace => userWorkspace.WorkspaceId == workspaceId && userWorkspace.UserId == userId);

    public IEnumerable<UserWorkspace> GetWorkspaceMembers(Guid workspaceId)
        => _context.UserWorkspaces
        .AsNoTracking().Include(workspace => workspace.User)
        .Where(workspace => workspace.WorkspaceId == workspaceId);
}
