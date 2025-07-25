using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Domain.Models;
using CleanArchitecture.Infrastructure.Data;

namespace CleanArchitecture.Infrastructure.Repositories;

public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
{
    readonly TaskifyDbContext _context;
    public NotificationRepository(TaskifyDbContext context) 
        : base(context) => _context = context;


    public IEnumerable<Notification> GetUserNotification(string userId)
        => _context.Notifications.Where(notification => notification.UserId == userId);
}
