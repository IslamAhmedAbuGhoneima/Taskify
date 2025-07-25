using AutoMapper;
using CleanArchitecture.Application.DTOs.NotificationDtos;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Application.Interfaces.Services;
using CleanArchitecture.Domain.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CleanArchitecture.Application.Implementations.Services;

public class NotificationService : INotificationService
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IHttpContextAccessor _httpContext;

    public NotificationService(IUnitOfWork unitOfWork, 
        IMapper mapper,
        IHttpContextAccessor httpContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContext = httpContext;
    }

    public async Task<NotificationDto> CreateNotification(NotificationForCreation request)
    {
        var notification = _mapper.Map<Notification>(request);

        await _unitOfWork.NotificationRepo.AddAsync(notification);

        await _unitOfWork.SaveAsync();

        var notificationDto = _mapper.Map<NotificationDto>(notification);

        return notificationDto;
    }

    public async Task<bool> DeleteNotificatino(Guid notificationId)
    {
        var userId = _httpContext.HttpContext?.User.FindFirstValue("id");

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("you are not authorized to do this actino");

        var notification = _unitOfWork.NotificationRepo.GetEntity(notificationId);

        if (notification == null)
            return false;

        if (notification.UserId != userId)
            throw new AccessDeniedException();

        _unitOfWork.NotificationRepo.Remove(notification);

        return await _unitOfWork.SaveAsync() > 0;
    }

    public IEnumerable<NotificationDto> GetAllUserNotifications()
    {
        var userId = _httpContext.HttpContext?.User.FindFirstValue("id");

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("you are not authorized to do this actino");


        var notifications = _unitOfWork.NotificationRepo.GetUserNotification(userId);

        var notificationsDto = _mapper.Map<IEnumerable<NotificationDto>>(notifications);

        return notificationsDto;
    }

    public NotificationDto GetNotification(Guid notificationId)
    {
        var notificatino = _unitOfWork.NotificationRepo.GetEntity(notificationId);

        if (notificatino == null)
            return null;

        var notificationDto = _mapper.Map<NotificationDto>(notificatino);

        return notificationDto;
    }

    public async Task<bool> MarkNotificationAsRead(Guid notificationId)
    {
        var userId = _httpContext.HttpContext?.User.FindFirstValue("id");

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("you are not authorized to do this actino");

        var notification = _unitOfWork.NotificationRepo.GetEntity(notificationId);

        if (notification == null)
            return false;

        if (notification.UserId != userId)
            throw new AccessDeniedException();

        notification.IsRead = true;

        return await _unitOfWork.SaveAsync() > 0;
    }
}

