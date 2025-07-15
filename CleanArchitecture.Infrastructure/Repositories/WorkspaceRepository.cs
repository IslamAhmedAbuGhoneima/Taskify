using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Domain.Models;
using CleanArchitecture.Infrastructure.Data;

namespace CleanArchitecture.Infrastructure.Repositories;

public class WorkspaceRepository : BaseRepository<Workspace>, IWorkspaceRepository
{
    public WorkspaceRepository(TaskifyDbContext context)
        : base(context) { }
}
