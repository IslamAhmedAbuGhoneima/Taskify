using AutoMapper;
using CleanArchitecture.Application.DTOs.WorkspaceDtos;
using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Application.Mapping;

public class WorkspaceProfile : Profile
{
    public WorkspaceProfile()
    {
        CreateMap<Workspace, WorkspaceDto>();

        CreateMap<WorkspacesForCreationDto, Workspace>()
            .ForMember(d => d.CreatedAt, 
            opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}
