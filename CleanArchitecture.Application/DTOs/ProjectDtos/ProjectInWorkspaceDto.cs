namespace CleanArchitecture.Application.DTOs.ProjectDtos;

public record ProjectInWorkspaceDto(
    Guid ProjectId,
    string Name,
    string? Description,
    DateTime CreatedAt,
    bool IsArchived,
    string? Color,
    int TaskCount,
    Guid WorkspaceId);
