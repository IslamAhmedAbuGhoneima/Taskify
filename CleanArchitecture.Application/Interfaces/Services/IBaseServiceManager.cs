namespace CleanArchitecture.Application.Interfaces.Services;

public interface IBaseServiceManager
{
    IWorkspaceService WorkspaceService { get; }

    IAuthenticationService AuthenticationService { get; }
}
