using CleanArchitecture.Application.DTOs.WorkspaceDtos;
using CleanArchitecture.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WorkspaceController(IBaseServiceManager serviceManager) : ControllerBase
{

    /// <summary>
    /// Get all workspaces created 
    /// </summary>
    /// <remarks>Returns a list of all workspaces created by users.</remarks>
    /// <returns>List of <see cref="WorkspaceDto"/></returns>
    /// <response code="200">Returns all workspaces</response>
    /// <response code="500">If there is any internal server error</response> 
    
    [HttpGet("workspaces")]
    public IActionResult GetWorkspaces()
    {
        try
        {
            var workspaces = serviceManager.WorkspaceService.GetAllWorkspaces();

            return Ok(workspaces);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Creates a new workspace and assigns the authenticated admin user as its owner.
    /// </summary>
    /// <param name="request">the workspace creation request <see cref="WorkspacesForCreationDto"/></param>
    /// <returns>the newly created workspace <see cref="WorkspaceDto"/></returns>
    /// <response code="201">Returns the created workspace</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not an admin</response>
    /// <response code="500">If there is any internal server error</response> 

    [Authorize(Roles = "admin")]
    [HttpPost("workspace-create")]
    public async Task<IActionResult> CreateWorkspace([FromBody] WorkspacesForCreationDto request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var workspace =
                await serviceManager.WorkspaceService.CreateWorkspace(request);

            return CreatedAtAction(nameof(GetWorkspaceById), new { id = workspace.Id }, workspace);
        }
        catch(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
        
    }



    /// <summary>
    /// Retrieves a workspace by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the workspace to retrieve. </param>
    /// <returns>The matching<see cref="WorkspaceDto"/>, if found</returns>
    /// <response code="200">Returns the workspace</response>
    /// <response code="404">If the workspace was not found</response>
    /// <response code="500">If there is any internal server error</response> 
    
    [Authorize]
    [HttpGet("{id:guid}/workspace")]
    public IActionResult GetWorkspaceById(Guid id)
    {
        try
        {
            var workspace = serviceManager.WorkspaceService.GetWorkspace(id);

            if (workspace is null)
                return NotFound();

            return Ok(workspace);
        }
        catch(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
        
    }


    /// <summary>
    /// Update workspace created by admin
    /// </summary>
    /// <param name="id">The Id of workspace you want to update</param> // 
    /// <param name="request">The request body <see cref="WorkspacesForUpdateDto"/></param>
    /// <response code="204">The workspace updated successfully</response> // 
    /// <response code="400">If the request is invalid</response> // 
    /// <response code="404">The workspace not found</response> // 
    /// <response code="500">If there is any internal server error</response> 

    [Authorize(Roles = "admin")]
    [HttpPut("{id:guid}/update")]
    public async Task<IActionResult> UpdateWorkspace(Guid id, [FromBody] WorkspacesForUpdateDto request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success =
                await serviceManager.WorkspaceService.UpdateWorkspace(id, request);

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
    /// Delete a single workspace
    /// </summary>
    /// <param name="id">The id of workspace you want to delete</param>
    /// <response code="204">The workspace deleted successfully</response>
    /// <response code="404">The workspace not found</response>
    /// <response code="500">If there is any internal server error</response> 

    [Authorize(Roles = "admin")]
    [HttpDelete("{id:guid}/delete")]
    public async Task<IActionResult> DeleteWorkspace(Guid id)
    {
        try
        {
            var success =
            await serviceManager.WorkspaceService.DeleteWorkspace(id);

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