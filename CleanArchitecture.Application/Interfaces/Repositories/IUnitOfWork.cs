namespace CleanArchitecture.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    IWorkspaceRepository WorkspaceRepo { get; }

    Task<int> SaveAsync();
}
