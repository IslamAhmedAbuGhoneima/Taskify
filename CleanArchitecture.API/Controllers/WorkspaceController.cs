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

    [HttpGet("workspaces")]
    public IActionResult Workspaces()
    {
        var workspaces = serviceManager.WorkspaceService.GetAllWorkspaces();

        return Ok(workspaces);
    }


    /// <summary>
    /// Creates a new workspace and assigns the authenticated admin user as its owner.
    /// </summary>
    /// <param name="request">the workspace creation request</param>
    /// <returns>the newly  created workspace <see cref="WorkspaceDto"/></returns>
    /// <response code="200">Returns the created workspace</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not an admin</response>
    [Authorize(Roles ="admin")]
    [HttpPost("workspace-create")]
    public async Task<IActionResult> CreateWorkspace([FromBody] WorkspacesForCreationDto request)
    {
        var workspace = 
            await serviceManager.WorkspaceService.CreateWorkspace(request);
            
        return Ok(workspace);
    }



    /// <summary>
    /// Retrieves a workspace by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the workspace to retrieve. </param>
    /// <returns>The matching<see cref="WorkspaceDto"/>, if found</returns>
    /// <response code="200">Returns the workspace</response>
    /// <response code="404">If the workspace was not found</response>
    [Authorize]
    [HttpGet("{id:guid}-workspace")]
    public IActionResult GetWorkspaceById(Guid id)
    {
        var workspace = serviceManager.WorkspaceService.GetWorkspace(id);

        if (workspace is null)
            return NotFound();

        return Ok(workspace);
    }
}
