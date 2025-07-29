namespace CleanArchitecture.Application.DTOs.LabelDtos;

public record LabelDto(Guid Id, string Name, string Color, DateTime CreatedAt, Guid WorkspaceId);
