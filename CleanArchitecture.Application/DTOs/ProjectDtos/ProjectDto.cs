namespace CleanArchitecture.Application.DTOs.ProjectDtos;

public record ProjectDto(Guid Id, string Name, string? Description, string? Color, DateTime CreatedAt, Guid WorkspaceId);