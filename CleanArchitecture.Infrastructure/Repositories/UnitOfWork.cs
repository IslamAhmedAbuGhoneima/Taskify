using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Infrastructure.Data;

namespace CleanArchitecture.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    readonly TaskifyDbContext _context;
    readonly IWorkspaceRepository _workspaceRepository;
    readonly IProjectRepository _projectRepository;

    public UnitOfWork(TaskifyDbContext context)
    {
        _context = context;
        _workspaceRepository = new WorkspaceRepository(context);
        _projectRepository = new ProjectRepository(context);
    }

    public IWorkspaceRepository WorkspaceRepo => _workspaceRepository;
    public IProjectRepository ProjectRepo => _projectRepository;
    
    public async Task<int> SaveAsync()
        => await _context.SaveChangesAsync();
}
