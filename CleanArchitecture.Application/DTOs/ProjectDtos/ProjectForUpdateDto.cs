namespace CleanArchitecture.Application.DTOs.ProjectDtos;

public record ProjectForUpdateDto(string Name,
    string? Description,
    string? Color,
    bool IsArchived);
