namespace CleanArchitecture.Application.DTOs.WorkspaceDtos;

public record WorkspaceDto(Guid Id, string Name, string Slug, string? Description, string OwnerId, DateTime CreatedAt);
