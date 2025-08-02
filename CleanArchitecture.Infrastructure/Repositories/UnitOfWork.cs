using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Infrastructure.Data;

namespace CleanArchitecture.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    readonly TaskifyDbContext _context;
    readonly IWorkspaceRepository _workspaceRepository;
    readonly IProjectRepository _projectRepository;
    readonly ITaskRepository _taskRepository;
    readonly ICommentRepository _commentRepository;
    readonly INotificationRepository _notificationRepository;
    readonly IAttachmentRepository _attachmentRepository;
    readonly ILabelRepository _labelRepository;
    readonly ITaskLabelRepository _taskLabelRepository;
    readonly IUserWorkspaceRepository _userWorkspaceRepository;


    public UnitOfWork(TaskifyDbContext context)
    {
        _context = context;
        _workspaceRepository = new WorkspaceRepository(context);
        _projectRepository = new ProjectRepository(context);
        _taskRepository = new TaskRepository(context);
        _commentRepository = new CommentRepository(context);
        _notificationRepository = new NotificationRepository(context);
        _attachmentRepository = new AttachmentRepository(context);
        _labelRepository = new LabelRepository(context);
        _taskLabelRepository = new TaskLabelRepository(context);
        _userWorkspaceRepository = new UserWorkspaceRepository(context);
    }

    public IWorkspaceRepository WorkspaceRepo => _workspaceRepository;

    public IProjectRepository ProjectRepo => _projectRepository;

    public ITaskRepository TaskRepo => _taskRepository;

    public ICommentRepository CommentRepo => _commentRepository;

    public INotificationRepository NotificationRepo => _notificationRepository;

    public IAttachmentRepository AttachmentRepo => _attachmentRepository;

    public ILabelRepository LableRepo => _labelRepository;

    public ITaskLabelRepository TaskLabelRepo => _taskLabelRepository;

    public IUserWorkspaceRepository UserWorkspaceRepo => _userWorkspaceRepository;

    public async Task<int> SaveAsync()
        => await _context.SaveChangesAsync();
}
