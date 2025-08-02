using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Application.Interfaces.Repositories;

public interface IUserWorkspaceRepository : IBaseRepository<UserWorkspace>
{
    IEnumerable<UserWorkspace> GetWorkspaceMembers(Guid workspaceId);

    UserWorkspace GetUserWorkspace(Guid workspaceId, string userId);
}
