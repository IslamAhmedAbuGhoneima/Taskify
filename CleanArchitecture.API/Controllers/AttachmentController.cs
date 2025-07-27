using CleanArchitecture.Application.DTOs.AttachmentDtos;
using CleanArchitecture.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AttachmentController
    (IBaseServiceManager serviceManager) : ControllerBase
{
    /// <summary>
    /// Upload Attachment related to specific task
    /// </summary>
    /// <param name="requst">The object for create attachment <see cref="AttachmentForCreationDto"/></param>
    /// <returns>The newly created attachment object <see cref="AttachmentDto"/> </returns>
    /// <response code="201">If the object created</response>
    /// <response code="400">If the object request is not correct</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="500">If there is any internal server error</response>

    [Authorize]
    [HttpPost("upload")]
    public async Task<IActionResult> UploadAttachment([FromForm] AttachmentForCreationDto requst)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var attachmentDto =
                await serviceManager.AttachmentService.UploadAttachment(requst);

            return CreatedAtAction(nameof(GetAttachment), new { attachmentId = attachmentDto.Id }, attachmentDto);

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Get Attachment Detalis
    /// </summary>
    /// <param name="attachmentId">Id of attachment you want to get</param>
    /// <returns>The attachment object <see cref="AttachmentDto"/></returns>
    /// <response code="200">If the object found</response>
    /// <response code="404">If the object not found</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="500">If there is any internal server error</response>

    [Authorize]
    [HttpGet("{attachmentId:guid}")]
    public IActionResult GetAttachment(Guid attachmentId)
    {
        try
        {
            var attachment =
                 serviceManager.AttachmentService.GetAttachment(attachmentId);

            if (attachment is null) 
                return NotFound();

            return Ok(attachment);

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Download specific file 
    /// </summary>
    /// <param name="attachmentId">Id of file you want to download</param>
    /// <returns>The file user clicked to download</returns>
    /// <response code="404">If the File not found</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="500">If there is any internal server error</response>

    [Authorize]
    [HttpGet("{attachmentId:guid}/download")]
    public IActionResult DownloadAttachment(Guid attachmentId)
    {
        try
        {
            var attachment =
                 serviceManager.AttachmentService.GetAttachment(attachmentId);

            if (attachment is null)
                return NotFound();

            if (!System.IO.File.Exists(attachment.BlobPath))
                return NotFound("File not found on server");

             var stream = 
                new FileStream(attachment.BlobPath, FileMode.Open, FileAccess.Read, FileShare.Read);

            Response.Headers.Append("Content-Disposition", $"attachment; filename=\"{attachment.FileName}\"");

            return File(stream, attachment.ContentType, attachment.FileName);

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Return List of attachments linked to task
    /// </summary>
    /// <param name="taskId">Id of task you want to get the attchment linked to it</param>
    /// <returns>List of attachments objects <see cref="IEnumerable{AttachmentDto}"/></returns>
    /// <response code="200">If the task has attachment file</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="404">If the task not found or it has no attachment</response>
    /// <response code="500">If there is any internal server error</response>

    [Authorize]
    [HttpGet("{taskId:guid}/attachments")]
    public IActionResult GetTaskAttachments(Guid taskId)
    {
        try
        {
            var taskAttachments =
                 serviceManager.AttachmentService.GetTaskAttachments(taskId);

            if (taskAttachments is null)
                return NotFound();

            return Ok(taskAttachments);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Delete attachment
    /// </summary>
    /// <param name="attachmenId">Id of the attachment you want to delete</param>
    /// <response code="204">If the attachment deleted</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="404">If the attachment not found</response>
    /// <response code="500">If there is any internal server error</response>

    [Authorize]
    [HttpDelete("{attachmenId:guid}/delete")]
    public async Task<IActionResult> DeleteAttachment(Guid attachmenId)
    {
        try
        {
            var success =
                 await serviceManager.AttachmentService.DeleteAttachment(attachmenId);

            if (!success) 
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

}
