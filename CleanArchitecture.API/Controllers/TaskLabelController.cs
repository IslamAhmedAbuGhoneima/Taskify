using CleanArchitecture.Application.DTOs.TaskLabelDtos;
using CleanArchitecture.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaskLabelController(IBaseServiceManager serviceManager) : ControllerBase
{

    /// <summary>
    /// Add label from specific task
    /// </summary>
    /// <param name="requst">the object request from add label for task <see cref="TaskLabelForCreationDto"/></param>
    /// <response code="200">If label added to task successfully </response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user has't the role to do this action</response>
    /// <response code="500">If there is any internal server error</response> 

    [HttpPost("Add-label-task")]
    [Authorize(Roles ="admin")]
    public async Task<IActionResult> AddLabelToTask(TaskLabelForCreationDto requst)
    {
        try
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var taskLabel = await serviceManager.TaskLabelService.AssociateLabelToTask(requst);
            return Ok();
        }
        catch (Exception ex)
        { 
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Delete label from task
    /// </summary>
    /// <param name="taskId">Id of task you want to delete its label</param>
    /// <param name="labelId">Id of label you want to delete it from task</param>
    /// <response code="204">If label removed successfully </response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user has't the role to do this action</response>
    /// <response code="500">If there is any internal server error</response> 

    [HttpDelete("{taskId:guid}/{labelId:guid}/remove-label")]
    [Authorize(Roles ="admin")]
    public async Task<IActionResult> RemoveLabelFromTask(Guid taskId, Guid labelId)
    {
        try
        {
            var deleted = await serviceManager.TaskLabelService.RemovelabelFromTask(taskId, labelId);

            if (!deleted)
                return NotFound("Failed to remove label from task");

            return NoContent();
        }
        catch(Exception ex) 
        {
            return StatusCode(500, ex.Message);
        }
        
    }


    /// <summary>
    /// Return List of Labels associated with task
    /// </summary>
    /// <param name="taskId">Id of task you want to get its labels</param>
    /// <returns>List of lebels added to this task</returns>
    /// <response code="200">List of Lebels added to task </response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="500">If there is any internal server error</response> 

    [HttpGet("{taskId:guid}/labels")]
    [Authorize]
    public IActionResult GetLabelsForTask(Guid taskId)
    {
        try
        {
            var labels = serviceManager.TaskLabelService.TaskLabels(taskId);
            return Ok(labels);
        }
        catch(Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
        
    }
}
