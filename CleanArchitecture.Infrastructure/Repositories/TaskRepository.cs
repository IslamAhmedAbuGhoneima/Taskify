using CleanArchitecture.Application.Interfaces.Repositories;
using TaskEntity = CleanArchitecture.Domain.Models.Task;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories;

public class TaskRepository : BaseRepository<TaskEntity>, ITaskRepository
{
    readonly TaskifyDbContext _context;
    public TaskRepository(TaskifyDbContext context) 
        : base(context) => _context = context;

    public IEnumerable<TaskEntity> GetProjectTasks(Guid projectId)
    {
        return _context.Tasks
            .Include(task => task.User)
            .Where(task => task.ProjectId == projectId);
    }
    
}
