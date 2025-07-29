using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Domain.Models;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories;

public class TaskLabelRepository : BaseRepository<TaskLabel>, ITaskLabelRepository
{
    readonly TaskifyDbContext _context;
    public TaskLabelRepository(TaskifyDbContext context) 
        : base(context) => _context = context;

    public TaskLabel? GetTaskLabel(Guid taskId, Guid labelId)
        => _context.TaskLabels
        .FirstOrDefault(taskLabel => taskLabel.TaskId == taskId && taskLabel.LableId == labelId);

    public IEnumerable<Label> GetTaskLabels(Guid taskId)
        => _context.TaskLabels
        .AsNoTracking()
        .Include(taskLabel => taskLabel.Label)
        .Where(taskLabel => taskLabel.TaskId == taskId)
        .Select(taskLabel => taskLabel.Label);
}
