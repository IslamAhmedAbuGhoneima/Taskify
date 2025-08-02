using AutoMapper;
using CleanArchitecture.Application.DTOs.WorkspaceDtos;
using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Application.Mapping;

public class UserWorkspaceProfile : Profile
{
    public UserWorkspaceProfile()
    {
        CreateMap<UserWorkspace, WorkspaceMemberDto>()
            .ForMember(d => d.UserId, opts => opts.MapFrom(src => src.UserId))
            .ForMember(d => d.FullName, opts => opts.MapFrom(src => src.User != null ? src.User.UserName : string.Empty))
            .ForMember(d => d.Email, opts => opts.MapFrom(src => src.User != null ? src.User.Email : string.Empty))
            .ForMember(d => d.Role, opts => opts.MapFrom(src => src.Role));
        

        CreateMap<WorkspaceMemberForCreationDto, UserWorkspace>()
            .ForMember(d => d.JoinedAt, opts => opts.MapFrom(src => DateTime.UtcNow));

        CreateMap<UserWorkspace, UserWorkspaceDto>();
    }
}

