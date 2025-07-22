using CleanArchitecture.Application.DTOs.CommentDtos;
using CleanArchitecture.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CleanArchitecture.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentController(IBaseServiceManager serviceManager) : ControllerBase
{

    /// <summary>
    /// retrive all comments that belongs to task
    /// </summary>
    /// <param name="taskId">Id of task you want to get its comments</param>
    /// <returns>List of Comments associated with task <see cref="IEnumerable{CommentDto}"/></returns>
    /// <response code="200">return the list of comments</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="500">If there is any internal server error</response> 

    [Authorize]
    [HttpGet("{taskId:guid}/comments")]
    public IActionResult GetTaskComments(Guid taskId)
    {
        try
        {
            var taksComments = serviceManager.CommentService.GetCommentsByTask(taskId);

            return Ok(taksComments);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


    /// <summary>
    /// create new Comment and associate it to a task
    /// </summary>
    /// <param name="request">the comment creation request <see cref="CommentForCreationDto"/> </param>
    /// <returns>the newly created Comment <see cref="CommentDto"/></returns>
    /// <response code="201">Returns the created comment</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="500">If there is any internal server error</response> 

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateComment(CommentForCreationDto request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var commentDto = await serviceManager.CommentService.CreateComment(request);

            return CreatedAtAction(nameof(GetComment), new { commentId = commentDto.Id }, commentDto);

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


    /// <summary>
    /// Retrive specific comment
    /// </summary>
    /// <param name="commentId">Id of comment you want to get</param>
    /// <returns>The returned object <see cref="CommentDto"/></returns>
    /// <response code="200">Return the comment</response>
    /// <response code="404">If the comment not found</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="500">If there is any internal server error</response> 
    
    [Authorize]
    [HttpGet("{commentId:Guid}/comment")]
    public IActionResult GetComment(Guid commentId)
    {
        try
        {
            var commentDto = serviceManager.CommentService.GetComment(commentId);

            if (commentDto == null)
                return NotFound();

            return Ok(commentDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


    /// <summary>
    /// Update Comment By its Id
    /// </summary>
    /// <param name="commentId">Id of the comment you want to update</param>
    /// <param name="request">The comment update request<see cref="CommentForUpdateDto"/></param>
    /// <response code="204">Comment updated successfully</response>
    /// <response code="404">If the comment not found</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="500">If there is any internal server error</response> 

    [Authorize]
    [HttpPut("{commentId:guid}/update")]
    public async Task<IActionResult> UpdateComment(Guid commentId, CommentForUpdateDto request)
    {
        try
        {
            var success = await serviceManager.CommentService.UpdateComment(commentId, request);

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
    /// Delete specific comment
    /// </summary>
    /// <param name="commentId">Id of comment you want to delete</param>
    /// <response code="204">Comment deleted successfully</response>
    /// <response code="404">If the comment not found</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="500">If there is any internal server error</response> 

    [Authorize]
    [HttpDelete("{commentId:guid}/delete")]
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        try
        {
            var success = await serviceManager.CommentService.DeleteComment(commentId);

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
