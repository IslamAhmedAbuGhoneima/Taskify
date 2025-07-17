using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Application.Interfaces.Repositories;

public interface IProjectRepository : IBaseRepository<Project>
{
    IEnumerable<Project> WorkspaceProjects(Guid workspaceId);
}
