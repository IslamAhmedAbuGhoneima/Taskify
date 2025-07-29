using Azure.Core;
using CleanArchitecture.Application.DTOs.LabelDtos;
using CleanArchitecture.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LabelController
    (IBaseServiceManager serviceManager): ControllerBase
{
    /// <summary>
    /// Get label by Id
    /// </summary>
    /// <param name="labelId">Id of label you want</param>
    /// <returns>The returned label object <see cref="LabelDto"/></returns>
    /// <response code="200">Return the label</response>
    /// <response code="404">If the label not found</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="500">If there is any internal server error</response> 

    [HttpGet("{labelId:guid}")]
    [Authorize]
    public IActionResult GetLabel(Guid labelId)
    {
        try
        {
            var label = serviceManager.ILableService.GetLabel(labelId);

            if(label == null) 
                return NotFound();

            return Ok(label);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("workspace/{workspaceId:guid}/labels")]
    [Authorize]
    public IActionResult GetWorkspaceLabels(Guid workspaceId)
    {
        try
        {
            var workspaceLabels = serviceManager.ILableService.GetWorkspaceLabels(workspaceId);

            return Ok(workspaceLabels);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }


    /// <summary>
    /// Create label for workspace
    /// </summary>
    /// <param name="request">The request object for make label <see cref="LabelForCreationDto"/></param>
    /// <returns>The created label object <see cref="LabelDto"/></returns>
    /// <response code="201">The label object created</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user has't the role to do this action</response>
    /// <response code="500">If there is any internal server error</response> 

    [HttpPost("create")]
    [Authorize(Roles ="admin")]
    public async Task<IActionResult> CreateLabel(LabelForCreationDto request) 
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var label = 
                await serviceManager.ILableService.CreateLabel(request);

            return CreatedAtAction(nameof(GetLabel), new { labelId = label.Id }, label);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Delete label by Id
    /// </summary>
    /// <param name="labelId">Id of label you want to delete</param>
    /// <response code="204">label deleted successfully</response>
    /// <response code="404">If the label not found</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user has't the role to do this action</response>
    /// <response code="500">If there is any internal server error</response> 

    [HttpDelete("{labelId:guid}/delete")]
    [Authorize(Roles ="admin")]
    public async Task<IActionResult> DeleteLabel(Guid labelId)
    {
        try
        {
            var deleted =
                await serviceManager.ILableService.DeleteLabel(labelId);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Update label 
    /// </summary>
    /// <param name="labelId">Id of label you want to update</param>
    /// <response code="204">label updated successfully</response>
    /// <response code="404">If the label not found</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user has't the role to do this action</response>
    /// <response code="500">If there is any internal server error</response> 

    [HttpPut("{labelId:guid}/update")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateLabel(Guid labelId, LabelForUpdateDto requst)
    {
        try
        {
            var updated =
                await serviceManager.ILableService.UpdateLabel(labelId, requst);

            if (!updated)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
