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
    IWorkspaceService _workspaceService;
    IAuthenticationService _authenticationService;

    public BaseServiceManager(IUnitOfWork unitOfWork, 
        UserManager<User> userManager,
        IHttpContextAccessor contextAccessor,
        IOptions<JwtConfiguration> jwtConfiguration,
        IMapper mapper)
    {
        _workspaceService = new WorkspaceService(unitOfWork,contextAccessor, mapper);
        _authenticationService = new AuthenticationService(userManager, jwtConfiguration, mapper);

    }

    public IWorkspaceService WorkspaceService => _workspaceService;

    public IAuthenticationService AuthenticationService => _authenticationService;
}
