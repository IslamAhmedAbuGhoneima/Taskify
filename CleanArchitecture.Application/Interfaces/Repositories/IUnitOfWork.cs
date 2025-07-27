namespace CleanArchitecture.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    IWorkspaceRepository WorkspaceRepo { get; }

    IProjectRepository ProjectRepo { get; }

    ITaskRepository TaskRepo { get; }

    ICommentRepository CommentRepo { get; }

    INotificationRepository NotificationRepo { get; }

    IAttachmentRepository AttachmentRepo { get; }

    Task<int> SaveAsync();
}
