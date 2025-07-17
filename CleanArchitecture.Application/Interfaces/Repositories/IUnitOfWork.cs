namespace CleanArchitecture.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    IWorkspaceRepository WorkspaceRepo { get; }
    IProjectRepository ProjectRepo { get; }

    Task<int> SaveAsync();
}
