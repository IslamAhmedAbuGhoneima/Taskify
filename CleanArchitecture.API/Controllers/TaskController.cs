using CleanArchitecture.Application.DTOs.TaskDtos;
using CleanArchitecture.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CleanArchitecture.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaskController(IBaseServiceManager serviceManager) : ControllerBase
{


    /// <summary>
    /// Create new Task 
    /// </summary>
    /// <param name="request">The task creation request <see cref="TaskForCreationDto"/></param>
    /// <returns>the newly created task <see cref="TaskInProjectDto"/> </returns>
    /// <response code="201">Returns the created task</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="500">If there is any internal server error</response>

    [Authorize]
    [HttpPost("task/create")]
    public async Task<IActionResult> CreateTask(TaskForCreationDto request)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var taskDto = await serviceManager.TaskService.CreateTask(request);

            return CreatedAtAction(nameof(GetTask), new { taskId = taskDto.TaskId }, taskDto);

        }
        catch(Exception ex) 
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

    }

    /// <summary>
    /// Retrive specific task
    /// </summary>
    /// <param name="taskId">Id of Task you want to get</param>
    /// <returns>The retrived object <see cref="TaskInProjectDto"/></returns>
    /// <response code="200">If the taks found</response>
    /// <response code="404">If the taks not found</response>
    /// <response code="500">If there is any internal server error</response>

    [Authorize]
    [HttpGet("{taskId:guid}/task")]
    public IActionResult GetTask(Guid taskId)
    {
        try
        {
            var taskDto = serviceManager.TaskService.GetTask(taskId);

            if (taskDto == null)
                return NotFound();

            return Ok(taskDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }



    /// <summary>
    /// Retrive the tasks that belong to specific project 
    /// </summary>
    /// <param name="projectId">Id of Project you want to retrive their tasks </param>
    /// <response code="200">The Tasks that belong to project or null in case there is not tasks attached to this project</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="500">If there is any internal server error</response>
    /// <returns>List of Tasks that belong to the project <see cref="IEnumerable{TaskInProjectDto}"/></returns>

    [Authorize]
    [HttpGet("{projectId:guid}/tasks")]
    public IActionResult GetProjectTasks(Guid projectId)
    {
        try
        {
            var projectTasks = serviceManager.TaskService.GetTasksByProject(projectId);
            return Ok(projectTasks);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

    }


    /// <summary>
    /// assign task to specific user
    /// </summary>
    /// <param name="taskId">Id of the task you want to assign it to user</param>
    /// <param name="userId">Id of the user you want to assign the task to</param>
    /// <response code="204">If the task assigned to user</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not allowed to do this action</response>
    /// <response code="404">If the task not found</response>
    /// <response code="500">If there is any internal server error</response>

    [Authorize(Roles = "admin")]
    [HttpPut("{taskId:guid}/assignToUser")]
    public async Task<IActionResult> AssingTaskToUser(Guid taskId, [FromBody] string userId)
    {
        try
        {
            var success = await serviceManager.TaskService.AssignTaskToUser(taskId, userId);

            if(!success)
                return NotFound();

            return NoContent();
        }
        catch(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


    /// <summary>
    /// Delete Specific task
    /// </summary>
    /// <param name="taskId">Id of task you want to delete</param>
    /// <response code="204">If the task deleted</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="403">If the user is not allowed to do this action</response>
    /// <response code="404">If the task not found</response>
    /// <response code="500">If there is any internal server error</response>

    [Authorize(Roles ="admin")]
    [HttpDelete("{taskId:guid}/delete")]
    public async Task<IActionResult> DeleteTask(Guid taskId)
    {
        try
        {
            var success = await serviceManager.TaskService.DeleteTask(taskId);

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
