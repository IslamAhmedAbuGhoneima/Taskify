using AutoMapper;
using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Application.Interfaces.Services;
using CleanArchitecture.Domain.ConfigurationModel;
using CleanArchitecture.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Application.Implementations.Services;

public class BaseServiceManager : IBaseServiceManager
{
    readonly IWorkspaceService _workspaceService;
    readonly IAuthenticationService _authenticationService;
    readonly IProjectService _projectService;
    readonly ITaskService _taskService;
    readonly ICommentService _commentService;
    readonly INotificationService _notificationService;

    public BaseServiceManager(IUnitOfWork unitOfWork,
        UserManager<User> userManager,
        IHttpContextAccessor contextAccessor,
        IOptions<JwtConfiguration> jwtConfiguration,
        IMapper mapper)
    {
        _workspaceService = new WorkspaceService(unitOfWork, contextAccessor, mapper);
        _authenticationService = new AuthenticationService(userManager, jwtConfiguration, mapper);
        _projectService = new ProjectService(unitOfWork, mapper);
        _taskService = new TaskService(unitOfWork, mapper);
        _commentService = new CommentService(unitOfWork, contextAccessor, mapper);
        _notificationService = new NotificationService(unitOfWork, mapper, contextAccessor);

    }

    public IWorkspaceService WorkspaceService => _workspaceService;

    public IAuthenticationService AuthenticationService => _authenticationService;

    public IProjectService ProjectService => _projectService;

    public ITaskService TaskService => _taskService;

    public ICommentService CommentService => _commentService;

    public INotificationService NotificationService => _notificationService;
}
