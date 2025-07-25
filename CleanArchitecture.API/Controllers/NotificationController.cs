using CleanArchitecture.Application.DTOs.NotificationDtos;
using CleanArchitecture.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CleanArchitecture.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationController(IBaseServiceManager serviceManager) : ControllerBase
{
    /// <summary>
    /// Create new Notification for a task
    /// </summary>
    /// <param name="request">the object we use to create new notifiacation <see cref="NotificationForCreation"/></param>
    /// <returns>the newly created notification object</returns>
    /// <response code="201">Returns the created notification</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="500">If there is any internal server error</response>

    [HttpPost("create")]
    public async Task<IActionResult> CreateNofification(NotificationForCreation request)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var notificationDto =
                        await serviceManager.NotificationService.CreateNotification(request);

            return CreatedAtAction(nameof(GetNotification), new { notificationId = notificationDto.Id }, notificationDto);

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        } 
    }

    /// <summary>
    /// Get notification by Id
    /// </summary>
    /// <param name="notificationId">Id of notification you want to get</param>
    /// <returns>The notification object <see cref="NotificationDto"/></returns>
    /// <response code="200">Returns the retrived notification if found</response>
    /// <response code="404">If the notification not found</response>
    /// <response code="500">If there is any internal server error</response>

    [HttpGet("{notificationId:guid}/notification")]
    public IActionResult GetNotification(Guid notificationId)
    {
        try
        {
            var notificationDto = 
                serviceManager.NotificationService.GetNotification(notificationId);

            if (notificationDto == null)
                return NotFound();

            return Ok(notificationDto);

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


    /// <summary>
    /// Get all notification sent to user
    /// </summary>
    /// <returns>List of all notificaiton user get <see cref="IEnumerable{NotificationDto}"/></returns>
    /// <response code="200">Returns the retrived notification</response>
    /// <response code="401">If the User not authenticated</response>
    /// <response code="500">If there is any internal server error</response>

    [Authorize]
    [HttpGet("notificatinos")]
    public IActionResult GetUserNotifications()
    {
        try
        {
            var notificationsDto =
                serviceManager.NotificationService.GetAllUserNotifications();

            return Ok(notificationsDto);

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Mark notification as read
    /// </summary>
    /// <param name="notificationId">Id of the notification you want to mark as read</param>
    /// <response code="204">If notification marked as read</response>
    /// <response code="401">If user not authorized</response>
    /// <response code="404">If the notification not found</response>
    /// <response code="500">If there is any internal server error</response>

    [Authorize]
    [HttpPut("{notificationId:guid}/mark-read")]
    public async Task<IActionResult> MarkNotificationAsRead(Guid notificationId)
    {
        try
        {
            var success = 
               await serviceManager.NotificationService.MarkNotificationAsRead(notificationId);

            if (!success) 
                return NotFound();

            return NoContent();

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Delete notification
    /// </summary>
    /// <param name="notificationId">Id of the notification you want to delete</param>
    /// <response code="204">If notification deleted</response>
    /// <response code="401">If user not authorized</response>
    /// <response code="404">If the notification not found</response>
    /// <response code="500">If there is any internal server error</response>

    [Authorize]
    [HttpDelete("{notificationId:guid}/delete")]
    public async Task<IActionResult> DeleteNotification(Guid notificationId)
    {
        try
        {
            var success =
               await serviceManager.NotificationService.DeleteNotificatino(notificationId);

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
