using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Application.Interfaces.Repositories;

public interface INotificationRepository : IBaseRepository<Notification>
{
    IEnumerable<Notification> GetUserNotification(string userId);
}
