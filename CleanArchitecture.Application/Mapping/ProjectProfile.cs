using AutoMapper;
using CleanArchitecture.Application.DTOs.ProjectDtos;
using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Application.Mapping;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ProjectForCreationDto, Project>()
            .ForMember(d => d.CreatedAt, opts => opts.MapFrom(src => DateTime.UtcNow));

        CreateMap<Project, ProjectDto>();

        CreateMap<Project, ProjectInWorkspaceDto>()
            .ConstructUsing(src => new ProjectInWorkspaceDto(
                src.Id,
                src.Name,
                src.Description,
                src.CreatedAt,
                src.ArchivedAt != null,
                src.Color,
                src.Tasks.Count,
                src.WorkspaceId
            ));
    }
}
