namespace CleanArchitecture.Application.DTOs.WorkspaceDtos;

public record WorkspaceMemberForCreationDto(string UserId, Guid WorkspaceId, string? Role);
