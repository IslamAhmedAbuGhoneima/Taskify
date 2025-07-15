namespace CleanArchitecture.Application.DTOs.WorkspaceDtos;

public record WorkspacesForCreationDto(
    string Name,
    string Slug,
    string? Description
);
