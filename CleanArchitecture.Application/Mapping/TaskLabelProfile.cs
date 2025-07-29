using AutoMapper;
using CleanArchitecture.Application.DTOs.TaskLabelDtos;
using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Application.Mapping;

public class TaskLabelProfile : Profile
{
    public TaskLabelProfile()
    {
        CreateMap<TaskLabelForCreationDto, TaskLabel>();
    }
}
