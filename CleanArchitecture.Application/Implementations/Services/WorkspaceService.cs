using AutoMapper;
using CleanArchitecture.Application.DTOs.WorkspaceDtos;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Application.Interfaces.Services;
using CleanArchitecture.Domain.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CleanArchitecture.Application.Implementations.Services;

public class WorkspaceService : IWorkspaceService
{
    readonly IHttpContextAccessor _httpContext;
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public WorkspaceService(IUnitOfWork unitOfWork,
        IHttpContextAccessor httpContext, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContext = httpContext;
    }

    public async Task<WorkspaceDto> CreateWorkspace(WorkspacesForCreationDto request)
    {
        var userId = _httpContext.HttpContext.User.FindFirstValue("id");

        if (string.IsNullOrEmpty(userId)) 
            throw new UnauthorizedAccessException("User ID not found in context");

        var workspace = _mapper.Map<Workspace>(request);
        workspace.OwnerId = userId;

        await _unitOfWork.WorkspaceRepo.AddAsync(workspace);

        await _unitOfWork.SaveAsync();

        var workspaceDto = _mapper.Map<WorkspaceDto>(workspace);

        return workspaceDto;
    }

    public async Task<bool> DeleteWorkspace(Guid id)
    {
        var userId = _httpContext.HttpContext.User.FindFirstValue("id");

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("Unable to delete workspace");

        var workspace = _unitOfWork.WorkspaceRepo.GetEntity(id);

        if (workspace == null)
            return false;

        if (workspace.OwnerId != userId)
            throw new AccessDeniedException();

        _unitOfWork.WorkspaceRepo.Remove(workspace);
        return await _unitOfWork.SaveAsync() > 0;
    }

    public IEnumerable<WorkspaceDto> GetAllWorkspaces()
    {
        var workspaces = _unitOfWork.WorkspaceRepo.GetAll();
        var workspacesDto = _mapper.Map<IEnumerable<WorkspaceDto>>(workspaces);
        return workspacesDto;
    }

    public WorkspaceDto? GetWorkspace(Guid id)
    {
        var workspce = _unitOfWork.WorkspaceRepo.GetEntity(id);

        if (workspce == null)
            return null;

        var workspaceDto = _mapper.Map<WorkspaceDto>(workspce);

        return workspaceDto;
    }

    public async Task<bool> UpdateWorkspace(Guid id, WorkspacesForUpdateDto request)
    {
        var userId = _httpContext.HttpContext.User.FindFirstValue("id");

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("Unable to update workspace");

        var workspace = _unitOfWork.WorkspaceRepo.GetEntity(id);

        if (workspace == null)
            return false;

        if(workspace.OwnerId != userId)
            throw new AccessDeniedException();

        _mapper.Map(request, workspace);

        _unitOfWork.WorkspaceRepo.Update(workspace);
        return await _unitOfWork.SaveAsync() > 0;
    }
}
