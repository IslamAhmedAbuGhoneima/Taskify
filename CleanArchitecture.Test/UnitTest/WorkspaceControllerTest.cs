using CleanArchitecture.API.Controllers;
using CleanArchitecture.Application.DTOs.WorkspaceDtos;
using CleanArchitecture.Application.Interfaces.Services;
using CleanArchitecture.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ThreadTask = System.Threading.Tasks.Task;

namespace CleanArchitecture.Test.UnitTest;

[TestFixture]
[Category("Workspace")]
public class WorkspaceControllerTest
{
    private Mock<IWorkspaceService> _mockWorkspaceService;
    private Mock<IBaseServiceManager> _mockServiceManager;
    private WorkspaceController _controller;

    [SetUp]
    public void SetUp()
    {
        _mockWorkspaceService = new Mock<IWorkspaceService>();
        _mockServiceManager = new Mock<IBaseServiceManager>();
        _mockServiceManager.Setup(x => x.WorkspaceService).Returns(_mockWorkspaceService.Object);

        _controller = new WorkspaceController(_mockServiceManager.Object);
    }

    [Test]
    public void GetWorkspaces_WhenCalled_ReturnsOkWithWorkspaces()
    {
        // Arrange
        var expectedWorkspaces = new List<WorkspaceDto>
        {
            new WorkspaceDto(Guid.NewGuid(), "Workspace 1", "workspace-1", "Description 1", "owner1", DateTime.UtcNow),
            new WorkspaceDto(Guid.NewGuid(), "Workspace 2", "workspace-2", "Description 2", "owner2", DateTime.UtcNow)
        };

        _mockServiceManager.Setup(workspace=>workspace.WorkspaceService.GetAllWorkspaces())
            .Returns(expectedWorkspaces);

        // Act
        var result = _controller.GetWorkspaces();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult?.Value, Is.EqualTo(expectedWorkspaces));
    }

    [Test]
    public void GetWorkspaceById_WithExistingId_ReturnsOkWithWorkspace()
    {
        var workspacId = Guid.NewGuid();

        var workspace = 
            new WorkspaceDto(workspacId, "workspace 1",  "workspace-1",  "this is workspace","OwnerId", DateTime.Now );

        _mockServiceManager.Setup(workspace => workspace.WorkspaceService.GetWorkspace(workspacId))
            .Returns(workspace);

        var result = _controller.GetWorkspaceById(workspacId);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());

        var okResult = result as OkObjectResult;

        Assert.That(okResult?.Value,Is.EqualTo(workspace));

    }

    [Test]
    public void GetWorkspaceById_ReturnsNotFound_WhenNotExist()
    {

        var id = Guid.NewGuid();

        _mockServiceManager.Setup(workspace => workspace.WorkspaceService.GetWorkspace(id))
            .Returns((WorkspaceDto)null);

        var result = _controller.GetWorkspaceById(id);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public void GetWorkspaces_WhenExceptionThrown_ReturnsInternalServerError()
    {
        var exceptionMessage = "Database connection failed";

        _mockServiceManager.Setup(workspace => workspace.WorkspaceService.GetAllWorkspaces())
            .Throws(new Exception(exceptionMessage));

        var result = _controller.GetWorkspaces();

        Assert.That(result, Is.InstanceOf<ObjectResult>());

        var objectResult = result as ObjectResult;
        Assert.That(exceptionMessage, Is.EqualTo(objectResult?.Value));
        Assert.That(StatusCodes.Status500InternalServerError, Is.EqualTo(objectResult?.StatusCode));
    }

    [Test]
    public async ThreadTask CreateWorkspace_WithValidRequest_ReturnsCreated()
    {
        var workspace = new WorkspacesForCreationDto("Workspace 1", "workspace-1", "this is a workspace");

        var workspaceDto = new WorkspaceDto(Guid.NewGuid(), "Workspace 1", "workspace-1", "this is a workspace", "i am the owner",DateTime.UtcNow);


        _mockWorkspaceService.Setup(x => x.CreateWorkspace(workspace))
            .ReturnsAsync(workspaceDto);

        var result = await _controller.CreateWorkspace(workspace);
        
        Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());

        var objectResult = result as CreatedAtActionResult;

        Assert.That(nameof(_controller.GetWorkspaceById), Is.EqualTo(objectResult?.ActionName));
        Assert.That(workspaceDto.Id, Is.EqualTo(objectResult?.RouteValues?["id"]));
        Assert.That(workspaceDto, Is.EqualTo(objectResult?.Value));
        
    }

    [Test]
    public async ThreadTask CreateWorkspace_WithInvalidModel_ReturnsBadRequest()
    {
        var request = new WorkspacesForCreationDto("","workspace-2","this is description");
        _controller.ModelState.AddModelError("Name", "Name is required");

        var result = await _controller.CreateWorkspace(request);

        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());

        var badRequestResult = result as BadRequestObjectResult;
        Assert.That(StatusCodes.Status400BadRequest, Is.EqualTo(badRequestResult?.StatusCode));
    }

    [Test]
    public async ThreadTask UpdateWorkspace_WithValidRequest_ReturnsNoContent()
    {
        var workspaceId = Guid.NewGuid();
        var updatedWorkspace = new WorkspacesForUpdateDto("New workspace", "new-workspace", "");

        _mockServiceManager.Setup(x => x.WorkspaceService.UpdateWorkspace(workspaceId, updatedWorkspace))
            .ReturnsAsync(true);

        var result = await _controller.UpdateWorkspace(workspaceId, updatedWorkspace);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async ThreadTask UpdateWorkspace_WithInvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var workspaceId = Guid.NewGuid();

        var request = new WorkspacesForUpdateDto(Name: "null",Slug: "",Description: "");

        _controller.ModelState.AddModelError("Name", "Name is required");

        // Act
        var result = await _controller.UpdateWorkspace(workspaceId, request);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async ThreadTask UpdateWorkspace_WithNonExistingWorkspace_ReturnsNotFound()
    {
        // Arrange
        var workspaceId = Guid.NewGuid();
        var request = new WorkspacesForUpdateDto("workspace", "slug", "");
        _mockWorkspaceService.Setup(x => x.UpdateWorkspace(workspaceId, request))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.UpdateWorkspace(workspaceId, request);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async ThreadTask DeleteWorkspace_WithExistingId_ReturnsNoContent()
    {
        // Arrange
        var workspaceId = Guid.NewGuid();
        _mockWorkspaceService.Setup(x => x.DeleteWorkspace(workspaceId)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteWorkspace(workspaceId);

        // Assert
        Assert.That(result,Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async ThreadTask DeleteWorkspace_WithNonExistingId_ReturnsNotFound()
    {
        // Arrange
        var workspaceId = Guid.NewGuid();
        _mockWorkspaceService.Setup(x => x.DeleteWorkspace(workspaceId)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteWorkspace(workspaceId);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async ThreadTask DeleteWorkspace_WhenExceptionThrown_ReturnsInternalServerError()
    {
        // Arrange
        var workspaceId = Guid.NewGuid();
        var exceptionMessage = "Delete failed";
        _mockWorkspaceService.Setup(x => x.DeleteWorkspace(workspaceId))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _controller.DeleteWorkspace(workspaceId);

        // Assert
        Assert.That(result, Is.InstanceOf<ObjectResult>());

        var objectResult = result as ObjectResult;
        Assert.That(StatusCodes.Status500InternalServerError,Is.EqualTo(objectResult?.StatusCode));
        Assert.That(exceptionMessage, Is.EqualTo(objectResult?.Value));
    }

    [Test]
    public void GetWorkspaceMembers_WhenCalled_ReturnsOkWithMembers()
    {
        var workspacedId = Guid.NewGuid();
        var expectedMembers = new List<WorkspaceMemberDto>
        {
            new WorkspaceMemberDto { UserId = "user1",  FullName = "Name 1", Email = "nam1@gmail.com",Role ="Member",JoinedAt = DateTime.Now },
            new WorkspaceMemberDto { UserId = "user2",  FullName = "Name 2", Email = "nam2@gmail.com",Role ="Admin",JoinedAt = DateTime.Now },
        };

        _mockServiceManager.Setup(x => x.WorkspaceService.WorkspaceMembers(workspacedId))
            .Returns(expectedMembers);


        var result = _controller.GetWorkspaceMembers(workspacedId);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());

        var okObjectResult = result as OkObjectResult;

        Assert.That(expectedMembers, Is.EqualTo(okObjectResult?.Value));
    }

    [Test]
    public async ThreadTask AddUserToWorkspace_WithValidRequest_ReturnsOkWithUserWorkspace()
    {
        // Arrange
        var request = new WorkspaceMemberForCreationDto(UserId: "user1", Guid.NewGuid(), Role: "member");
        var expectedResult = new UserWorkspaceDto(UserId: "user1", Role: "member");

        _mockWorkspaceService.Setup(x => x.AddToWorkspace(request))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.AddUserToWorkspace(request);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());

        var okResult = result as OkObjectResult;

        Assert.That(expectedResult, Is.EqualTo(okResult?.Value));
    }

}
