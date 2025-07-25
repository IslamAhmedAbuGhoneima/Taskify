namespace CleanArchitecture.Application.DTOs.NotificationDtos;

public record NotificationForCreation(string Message, string Type,string UserId, Guid TargetTaskId);