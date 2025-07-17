using AutoMapper;
using CleanArchitecture.Application.DTOs.ProjectDtos;
using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Application.Interfaces.Services;
using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Application.Implementations.Services;

public class ProjectService : IProjectService
{
    IUnitOfWork _unitOfWork;
    IMapper _mapper;

    public ProjectService(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProjectDto> CreateProject(ProjectForCreationDto request)
    {
        var project = _mapper.Map<Project>(request);

        await _unitOfWork.ProjectRepo.AddAsync(project);

        await _unitOfWork.SaveAsync();

        var projectDto = _mapper.Map<ProjectDto>(project);

        return projectDto;
    }

    public async Task<bool> DeleteProject(Guid id)
    {
        var project = _unitOfWork.ProjectRepo.GetEntity(id);

        if (project == null)
            return false;

        project.ArchivedAt = DateTime.UtcNow;

        return await _unitOfWork.SaveAsync() > 0;
    }

    public ProjectDto? GetProjectById(Guid id)
    {
        var project = _unitOfWork.ProjectRepo.GetEntity(id);

        if (project == null)
            return null;

        var projectDto = _mapper.Map<ProjectDto>(project);

        return projectDto;
    }

    public IEnumerable<ProjectInWorkspaceDto> GetWorkspaceProjects(Guid workspaceId)
    {
        var workspaceProjects = _unitOfWork.ProjectRepo.WorkspaceProjects(workspaceId);

        var workspaceProjectsDto = _mapper.Map<IEnumerable<ProjectInWorkspaceDto>>(workspaceProjects);

        return workspaceProjectsDto;
    }

    public async Task<bool> UpdateProject(Guid id,ProjectForUpdateDto request)
    {
        var project = _unitOfWork.ProjectRepo.GetEntity(id);

        if (project is null)
            return false;

        _mapper.Map(request, project);

        _unitOfWork.ProjectRepo.Update(project);

        return await _unitOfWork.SaveAsync() > 0;
    }
}
