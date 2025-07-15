using AutoMapper;
using CleanArchitecture.Application.DTOs.WorkspaceDtos;
using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Application.Interfaces.Services;
using CleanArchitecture.Domain.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CleanArchitecture.Application.Implementations.Services;

public class WorkspaceService : IWorkspaceService
{
    IHttpContextAccessor _httpContext;
    IUnitOfWork _unitOfWork;
    IMapper _mapper;

    public WorkspaceService(IUnitOfWork unitOfWork,
        IHttpContextAccessor httpContext, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContext = httpContext;
    }

    public async Task<WorkspaceDto> CreateWorkspace(WorkspacesForCreationDto request)
    {
        var userId = _httpContext.HttpContext!.User.FindFirstValue("id");
        var workspace = _mapper.Map<Workspace>(request);
        workspace.OwnerId = userId!;

        await _unitOfWork.WorkspaceRepo.AddAsync(workspace);

        await _unitOfWork.SaveAsync();

        var workspaceDto = _mapper.Map<WorkspaceDto>(workspace);

        return workspaceDto;
    }

    public IEnumerable<WorkspaceDto> GetAllWorkspaces()
    {
        var workspaces = _unitOfWork.WorkspaceRepo.GetAll();
        var workspacesDto = _mapper.Map<IEnumerable<WorkspaceDto>>(workspaces);
        return workspacesDto;
    }

    public WorkspaceDto GetWorkspace(Guid id)
    {
        var workspce = _unitOfWork.WorkspaceRepo.GetEntity(id);

        var workspaceDto = _mapper.Map<WorkspaceDto>(workspce);

        return workspaceDto;
    }
}
