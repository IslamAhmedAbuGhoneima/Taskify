namespace CleanArchitecture.Application.Interfaces.Services;

public interface IBaseServiceManager
{
    IWorkspaceService WorkspaceService { get; }

    IProjectService ProjectService { get; }

    IAuthenticationService AuthenticationService { get; }

    ITaskService TaskService { get; }

    ICommentService CommentService { get; }

    INotificationService NotificationService { get; }

    IAttachmentService AttachmentService { get; }

    ILabelService ILableService { get; }

    ITaskLabelService TaskLabelService { get; }
}
