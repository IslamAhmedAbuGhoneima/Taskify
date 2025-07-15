using CleanArchitecture.Application.DTOs.WorkspaceDtos;

namespace CleanArchitecture.Application.Interfaces.Services;

public interface IWorkspaceService
{
    IEnumerable<WorkspaceDto> GetAllWorkspaces();

    Task<WorkspaceDto> CreateWorkspace(WorkspacesForCreationDto request);

    WorkspaceDto GetWorkspace(Guid id);
}
