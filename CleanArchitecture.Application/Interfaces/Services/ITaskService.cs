using CleanArchitecture.Application.DTOs.TaskDtos;

namespace CleanArchitecture.Application.Interfaces.Services;

public interface ITaskService 
{
    IEnumerable<TaskInProjectDto> GetTasksByProject(Guid projectId);

    Task<TaskInProjectDto> CreateTask(TaskForCreationDto request);

    TaskInProjectDto GetTask(Guid taskId);

    Task<bool> DeleteTask(Guid taskId);

    Task<bool> AssignTaskToUser(Guid taskId, string userId);
}
