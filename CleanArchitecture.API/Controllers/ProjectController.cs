using CleanArchitecture.Application.DTOs.ProjectDtos;
using CleanArchitecture.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController(IBaseServiceManager serviceManager) : ControllerBase
{

    /// <summary>
    /// Create new project and assign it to workspace
    /// </summary>
    /// <param name="request">The request for create project <see cref="ProjectForCreationDto"/></param>
    /// <returns>the newly  created workspace<see cref="ProjectDto"/></returns>
    /// <response code="201">Returns the created workspace</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="500">If there is any internal server error</response> 

    [Authorize]
    [HttpPost("create-project")]
    public async Task<IActionResult> CreateProject(ProjectForCreationDto request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var project =
                await serviceManager.ProjectService.CreateProject(request);

            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

    }

    /// <summary>
    /// Retrive the project by its unique identifier.
    /// </summary>
    /// <param name="id">The Id of Project to retrive</param>
    /// <returns>The matching peoject <see cref="ProjectDto"/> ,If Found</returns>
    /// <response code="200">If project found</response>
    /// <response code="404">If project not found</response>
    /// <response code="500">If there is any internal server error</response>

    [Authorize]
    [HttpGet("{id:guid}/project")]
    public IActionResult GetProject(Guid id)
    {
        try
        {
            var project = serviceManager.ProjectService.GetProjectById(id);

            if (project == null)
                return NotFound();

            return Ok(project);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

    }


    /// <summary>
    /// Retrive the project that belong to specific workspace
    /// </summary>
    /// <param name="workspaceId">The Id of workspace you want to retrive their projects</param>
    /// <returns>List of Projects that belong to the workspace <see cref="ProjectInWorkspaceDto"/></returns>
    /// <response code="200">If workspace has projects </response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="500">If there is any internal server error</response>

    [Authorize]
    [HttpGet("{workspaceId:guid}/projects")]
    public IActionResult GetWorkspaceProjects(Guid workspaceId)
    {
        try
        {
            var workspaceProjects = serviceManager.ProjectService.GetWorkspaceProjects(workspaceId);
            return Ok(workspaceProjects);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Archive Specific project by its Id
    /// </summary>
    /// <param name="id">The Id of project you want to archive</param>
    /// <response code="202">If the project archived</response>
    /// <response code="404">If the project not found</response>
    /// <response code="500">If there is any internal server error</response>

    [Authorize]
    [HttpDelete("{id:guid}/archive")]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        try
        {
            var success = await serviceManager.ProjectService.DeleteProject(id);

            if (!success)
                return NotFound();

            return NoContent();
        }
        catch(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


    /// <summary>
    /// Update Specific project
    /// </summary>
    /// <param name="id">The Id of project you want to update</param>
    /// <param name="request">The request object that has fields you want to update <see cref="ProjectForUpdateDto"/></param>
    /// <response code="202">If the project Updated successfully</response>
    /// <response code="400">If the request object has any invalid fields</response>
    /// <response code="404">If the project not found</response>
    /// <response code="500">If there is any internal server error</response>

    [Authorize]
    [HttpPut("{id:guid}/project/update")]
    public async Task<IActionResult> UpdateProject(Guid id, ProjectForUpdateDto request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await serviceManager.ProjectService.UpdateProject(id, request);

            if (!success)
                return NotFound();

            return NoContent();
        }
        catch(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
