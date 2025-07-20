using TaskEntity = CleanArchitecture.Domain.Models.Task;
namespace CleanArchitecture.Application.Interfaces.Repositories;

public interface ITaskRepository : IBaseRepository<TaskEntity>
{
    IEnumerable<TaskEntity> GetProjectTasks(Guid projectId);
}
