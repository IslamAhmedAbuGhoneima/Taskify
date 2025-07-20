using AutoMapper;
using CleanArchitecture.Application.DTOs.TaskDtos;
using CleanArchitecture.Domain.Enums;
using TaskEntity = CleanArchitecture.Domain.Models.Task;
namespace CleanArchitecture.Application.Mapping;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<TaskForCreationDto, TaskEntity>()
            .ForMember(d => d.CreatedAt, opts => opts.MapFrom(src => DateTime.UtcNow));


        CreateMap<TaskEntity, TaskInProjectDto>()
            .ForMember(d => d.TaskId, opts => opts.MapFrom(src => src.Id))
            .ForMember(d => d.AssignedToUserName, opts => opts.MapFrom(src => src.User.UserName))
            .ForMember(d => d.IsOverdue, opts => opts.MapFrom(src => src.DueDate.HasValue && DateTime.UtcNow > src.DueDate.Value && src.Status != Status.Done));
    }
}
