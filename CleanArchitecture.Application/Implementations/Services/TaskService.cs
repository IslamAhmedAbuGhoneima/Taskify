using AutoMapper;
using CleanArchitecture.Application.DTOs.TaskDtos;
using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Application.Interfaces.Services;
using TaskEntity = CleanArchitecture.Domain.Models.Task;

namespace CleanArchitecture.Application.Implementations.Services;

public class TaskService : ITaskService
{

    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public TaskService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> AssignTaskToUser(Guid taskId, string userId)
    {
        var task = _unitOfWork.TaskRepo.GetEntity(taskId);

        if (task == null)
            return false;

        task.AssignedToUserId = userId;

        return await _unitOfWork.SaveAsync() > 0;
    }

    public async Task<TaskInProjectDto> CreateTask(TaskForCreationDto request)
    {
        var task = _mapper.Map<TaskEntity>(request);

        await _unitOfWork.TaskRepo.AddAsync(task);

        await _unitOfWork.SaveAsync();

        var taskDto = _mapper.Map<TaskInProjectDto>(task);

        return taskDto;
    }

    public async Task<bool> DeleteTask(Guid taskId)
    {
        var task = _unitOfWork.TaskRepo.GetEntity(taskId);

        if (task == null)
            return false;

        _unitOfWork.TaskRepo.Remove(task);
        return await _unitOfWork.SaveAsync() > 0;
            
    }

    public TaskInProjectDto GetTask(Guid taskId)
    {
        var task = _unitOfWork.TaskRepo.GetEntity(taskId);

        var taskDto = _mapper.Map<TaskInProjectDto>(task);

        return taskDto;
    }

    public IEnumerable<TaskInProjectDto> GetTasksByProject(Guid projectId)
    {
        var projectTasks = _unitOfWork.TaskRepo.GetProjectTasks(projectId);

        var projectTasksDto = _mapper.Map<IEnumerable<TaskInProjectDto>>(projectTasks);

        return projectTasksDto;
    }
}
