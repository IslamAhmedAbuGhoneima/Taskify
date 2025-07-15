using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Infrastructure.Data;

namespace CleanArchitecture.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    TaskifyDbContext _context;
    IWorkspaceRepository _workspaceRepository;

    public UnitOfWork(TaskifyDbContext context)
    {
        _context = context;
        _workspaceRepository = new WorkspaceRepository(context);
    }

    public IWorkspaceRepository WorkspaceRepo => _workspaceRepository;
    
    public async Task<int> SaveAsync()
        => await _context.SaveChangesAsync();
}
