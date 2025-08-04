using CleanArchitecture.API.Controllers;
using CleanArchitecture.Application.DTOs.WorkspaceDtos;
using CleanArchitecture.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;


namespace CleanArchitecture.Test.UnitTest;

[TestFixture]
public class WorkspaceServiceTest
{
    private Mock<IWorkspaceService> _mockWorkspaceService;
    private Mock<IBaseServiceManager> _mockServiceManager;
    private WorkspaceController _controller;

    [OneTimeSetUp]
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


}
