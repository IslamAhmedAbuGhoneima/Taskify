namespace CleanArchitecture.Application.DTOs.TaskDtos;

public record TaskForCreationDto(string Title,
    string? Description,
    string Status,
    string Priority,
    DateTime? DueDate,
    int OrderIndex,
    Guid ProjectId,
    string? AssignedToUserId,
    List<Guid>? LabelIds);
