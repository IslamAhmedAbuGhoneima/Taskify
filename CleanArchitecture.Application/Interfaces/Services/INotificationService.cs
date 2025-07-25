using CleanArchitecture.Application.DTOs.NotificationDtos;

namespace CleanArchitecture.Application.Interfaces.Services;

public interface INotificationService
{
    Task<NotificationDto> CreateNotification(NotificationForCreation request);

    NotificationDto GetNotification(Guid notificationId);

    IEnumerable<NotificationDto> GetAllUserNotifications();

    Task<bool> MarkNotificationAsRead(Guid notificationId);

    Task<bool> DeleteNotificatino(Guid notificationId);
}
