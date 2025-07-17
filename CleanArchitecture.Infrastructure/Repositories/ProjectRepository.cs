using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Domain.Models;
using CleanArchitecture.Infrastructure.Data;

namespace CleanArchitecture.Infrastructure.Repositories;

public class ProjectRepository : BaseRepository<Project>, IProjectRepository
{
    readonly TaskifyDbContext _context;

    public ProjectRepository(TaskifyDbContext context) 
        : base(context) => _context = context;


    public IEnumerable<Project> WorkspaceProjects(Guid workspaceId)
        => _context.Projects.Where(project => project.WorkspaceId == workspaceId).ToList(); 
}
