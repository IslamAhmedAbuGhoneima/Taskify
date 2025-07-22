﻿namespace CleanArchitecture.Application.Interfaces.Services;

public interface IBaseServiceManager
{
    IWorkspaceService WorkspaceService { get; }

    IProjectService ProjectService { get; }

    IAuthenticationService AuthenticationService { get; }

    ITaskService TaskService { get; }

    ICommentService CommentService { get; }
}
