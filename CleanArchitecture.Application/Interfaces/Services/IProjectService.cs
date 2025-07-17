using CleanArchitecture.Application.DTOs.ProjectDtos;

namespace CleanArchitecture.Application.Interfaces.Services;

public interface IProjectService
{
    Task<ProjectDto> CreateProject(ProjectForCreationDto request);

    ProjectDto? GetProjectById(Guid id);

    IEnumerable<ProjectInWorkspaceDto> GetWorkspaceProjects(Guid workspaceId);

    Task<bool> DeleteProject(Guid id);

    Task<bool> UpdateProject(Guid id, ProjectForUpdateDto request);
}
