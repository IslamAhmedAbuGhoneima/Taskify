namespace CleanArchitecture.Application.DTOs.ProjectDtos;

public record ProjectForCreationDto(string Name, string? Description, string? Color, Guid WorkspaceId);