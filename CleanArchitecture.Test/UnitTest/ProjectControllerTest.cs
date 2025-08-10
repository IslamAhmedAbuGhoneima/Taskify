using CleanArchitecture.API.Controllers;
using CleanArchitecture.Application.DTOs.ProjectDtos;
using CleanArchitecture.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;

namespace CleanArchitecture.Test.UnitTest;

[TestFixture]
[Category("Project")]
public class ProjectControllerTest
{
    private Mock<IBaseServiceManager> _mockServiceManager;
    private Mock<IProjectService> _mockProjectService;
    private ProjectController _controller;

    [SetUp]
    public void SetUp()
    {
        _mockServiceManager = new Mock<IBaseServiceManager>();
        _mockProjectService = new Mock<IProjectService>();

        _mockServiceManager.Setup(x => x.ProjectService)
            .Returns(_mockProjectService.Object);

        _controller = new ProjectController(_mockServiceManager.Object);
    }

    [Test]
    public async Task CreateProject_WithValidRequest_ReturnsCreatedAtAction()
    {
        var workspaceId = Guid.NewGuid();
        var request = 
            new ProjectForCreationDto("project 1", "this is project", "#0000", workspaceId);


        var expectedProject = new ProjectDto(Guid.NewGuid(), "project 1", "this is project", "#0000",DateTime.UtcNow, workspaceId);

        _mockServiceManager.Setup(x => x.ProjectService.CreateProject(request))
            .ReturnsAsync(expectedProject);


        var result = await _controller.CreateProject(request);
        Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
        
        var createdResult = result as CreatedAtActionResult;
        Assert.That(createdResult?.ActionName, Is.EqualTo(nameof(_controller.GetProject)));
        Assert.That(createdResult?.RouteValues?["id"], Is.EqualTo(expectedProject.Id));
        Assert.That(createdResult?.Value, Is.EqualTo(expectedProject));
        _mockProjectService.Verify(x => x.CreateProject(request), Times.Once());
    }

    [Test]
    public async Task CreateProject_WithInvalidModel_ReturnsBadRequest()
    {
        var request = It.IsAny<ProjectForCreationDto>();

        _controller.ModelState.AddModelError("Name", "Name is required");

        var result = await _controller.CreateProject(request);

        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());

        var badRequestObject = result as BadRequestObjectResult;
        Assert.That(badRequestObject?.Value, Is.InstanceOf<SerializableError>());

        _mockProjectService.Verify(x => x.CreateProject(It.IsAny<ProjectForCreationDto>()), Times.Never);

    }

    [Test]
    public async Task CreateProject_WhenExceptionThrown_ReturnsInternalServerError()
    {
        var request = It.IsAny<ProjectForCreationDto>();

        var exceptionMessage = "Database connection failed";

        _mockProjectService.Setup(x => x.CreateProject(request))
            .Throws(new Exception(exceptionMessage));

        var result = await _controller.CreateProject(request);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;

        Assert.That(objectResult?.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        Assert.That(objectResult?.Value, Is.EqualTo(exceptionMessage));

    }

    [Test]
    public void GetProject_WithExistingId_ReturnsOkWithProject()
    {
        var projectId = Guid.NewGuid();
        var workspaceId = Guid.NewGuid();

        var expectedProject = new ProjectDto(
                projectId,
                "Test Project",
                "test-project",
                "#000455",
                DateTime.UtcNow,
                workspaceId);

        _mockProjectService.Setup(x => x.GetProjectById(projectId))
            .Returns(expectedProject);

        var result = _controller.GetProject(projectId);
        Assert.That(result, Is.InstanceOf<OkObjectResult>());

        var okObjectResult = result as OkObjectResult;
        Assert.That(okObjectResult?.Value, Is.EqualTo(expectedProject));
        _mockProjectService.Verify(x => x.GetProjectById(projectId), Times.Once());
    }

    [Test]
    public void GetProject_WithNonExistingId_ReturnsNotFound()
    {
        var projectId = Guid.NewGuid();

        _mockProjectService.Setup(x => x.GetProjectById(projectId))
            .Returns((ProjectDto)null);

        var result = _controller.GetProject(projectId);
        Assert.That(result, Is.InstanceOf<NotFoundResult>());

        var notfoundObject = result as NotFoundResult;
        Assert.That(notfoundObject?.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        _mockProjectService.Verify(x => x.GetProjectById(projectId), Times.Once);
    }

    [Test]
    public void GetProject_WhenExceptionThrown_ReturnsInternalServerError()
    {
        var projectId = Guid.NewGuid();
        var exceptionMessage = "Database error";

        _mockProjectService.Setup(x => x.GetProjectById(projectId))
            .Throws(new Exception(exceptionMessage));

        var result = _controller.GetProject(projectId);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        Assert.That(objectResult?.Value, Is.EqualTo(exceptionMessage));
    }

    [Test]
    public void GetWorkspaceProjects_WithValidWorkspaceId_ReturnsOkWithProjects()
    {
        var workspaceId = Guid.NewGuid();
        var expectedProjecsInWorkspace = new List<ProjectInWorkspaceDto>
        {
            new ProjectInWorkspaceDto(Guid.NewGuid(), "Project 1", "project-1", DateTime.UtcNow,false,"#523ba2",5, workspaceId),
            new ProjectInWorkspaceDto(Guid.NewGuid(), "Project 2", "project-2", DateTime.UtcNow,false,"#510435",2, workspaceId)
        };

        _mockProjectService.Setup(x => x.GetWorkspaceProjects(workspaceId))
            .Returns(expectedProjecsInWorkspace);

        var result = _controller.GetWorkspaceProjects(workspaceId);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());

        var okObjectResult = result as OkObjectResult;

        Assert.That(okObjectResult?.Value, Is.EqualTo(expectedProjecsInWorkspace));
        _mockProjectService.Verify(x => x.GetWorkspaceProjects(workspaceId), Times.Once);
    }

    [Test]
    public void GetWorkspaceProjects_WhenExceptionThrown_ReturnsInternalServerError()
    {
        var workspaceId = Guid.NewGuid();
        var exceptionMessage = "Failed to retrieve projects";
        _mockProjectService.Setup(x => x.GetWorkspaceProjects(workspaceId)).Throws(new Exception(exceptionMessage));

        var result = _controller.GetWorkspaceProjects(workspaceId);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        Assert.That(objectResult?.Value, Is.EqualTo(exceptionMessage));
    }

    [Test]
    public async Task DeleteProject_WithExistingId_ReturnsNoContent()
    {
        var projectId = Guid.NewGuid();
        _mockProjectService.Setup(x => x.DeleteProject(projectId)).ReturnsAsync(true);

        var result = await _controller.DeleteProject(projectId);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
        _mockProjectService.Verify(x => x.DeleteProject(projectId), Times.Once);
    }

    [Test]
    public async Task DeleteProject_WithNonExistingId_ReturnsNotFound()
    {
        var projectId = Guid.NewGuid();
        _mockProjectService.Setup(x => x.DeleteProject(projectId)).ReturnsAsync(false);

        var result = await _controller.DeleteProject(projectId);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
        _mockProjectService.Verify(x => x.DeleteProject(projectId), Times.Once);
    }

    [Test]
    public async Task DeleteProject_WhenExceptionThrown_ReturnsInternalServerError()
    {
        var projectId = Guid.NewGuid();
        var exceptionMessage = "Delete operation failed";
        _mockProjectService.Setup(x => x.DeleteProject(projectId)).ThrowsAsync(new Exception(exceptionMessage));

        var result = await _controller.DeleteProject(projectId);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult?.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        Assert.That(objectResult?.Value, Is.EqualTo(exceptionMessage));
    }

    [Test]
    public async Task UpdateProject_WithValidRequest_ReturnsNoContent()
    {
        var projectId = Guid.NewGuid();
        var request = new ProjectForUpdateDto("Updated Project", "Updated description", "#015365", false);
      
        _mockProjectService.Setup(x => x.UpdateProject(projectId, request)).ReturnsAsync(true);

        var result = await _controller.UpdateProject(projectId, request);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
        _mockProjectService.Verify(x => x.UpdateProject(projectId, request), Times.Once);
    }

    [Test]
    public async Task UpdateProject_WithInvalidModel_ReturnsBadRequest()
    {
        var projectId = Guid.NewGuid();
        var request = new ProjectForUpdateDto(Name: null,Description: "This is description",Color: "#158d8a",IsArchived: true);
        _controller.ModelState.AddModelError("Name", "Name is required");

         
        var result = await _controller.UpdateProject(projectId, request);

        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result as BadRequestObjectResult;
        Assert.That(badRequestResult?.Value, Is.InstanceOf<SerializableError>());
        _mockProjectService.Verify(x => x.UpdateProject(It.IsAny<Guid>(), It.IsAny<ProjectForUpdateDto>()), Times.Never);
    }

    [Test]
    public async Task UpdateProject_WithNonExistingProject_ReturnsNotFound()
    {
        var projectId = Guid.NewGuid();
        var request = new ProjectForUpdateDto("Updated Project", "Updated description", "#000000", false);

        _mockProjectService.Setup(x => x.UpdateProject(projectId, request))
            .ReturnsAsync(false);

        var result = await _controller.UpdateProject(projectId, request);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
        _mockProjectService.Verify(x => x.UpdateProject(projectId, request), Times.Once);
    }
}
