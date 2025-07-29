using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Application.Interfaces.Repositories;

public interface ITaskLabelRepository : IBaseRepository<TaskLabel>
{
    IEnumerable<Label> GetTaskLabels(Guid taskId);

    TaskLabel? GetTaskLabel(Guid taskId, Guid labelId);
}
