namespace CleanArchitecture.Application.DTOs.NotificationDtos;

public record NotificationDto(Guid Id,
    string Message,
    DateTime CreatedAt,
    string UserId,
    string Type,
    Guid? TargetTaskId,
    bool IsRead);
