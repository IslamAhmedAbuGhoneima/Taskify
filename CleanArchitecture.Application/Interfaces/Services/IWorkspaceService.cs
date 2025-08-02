using CleanArchitecture.Application.DTOs.WorkspaceDtos;

namespace CleanArchitecture.Application.Interfaces.Services;

public interface IWorkspaceService
{
    IEnumerable<WorkspaceDto> GetAllWorkspaces();

    Task<WorkspaceDto> CreateWorkspace(WorkspacesForCreationDto request);

    WorkspaceDto GetWorkspace(Guid id);

    IEnumerable<WorkspaceMemberDto> WorkspaceMembers(Guid workspaceId);

    Task<UserWorkspaceDto> AddToWorkspace(WorkspaceMemberForCreationDto request);

    Task<bool> ConvertUserWorkspaceRoleToAdmin(Guid workspaceId, string userId);

    Task<bool> UpdateWorkspace(Guid id, WorkspacesForUpdateDto request);

    Task<bool> DeleteWorkspace(Guid id);
}
