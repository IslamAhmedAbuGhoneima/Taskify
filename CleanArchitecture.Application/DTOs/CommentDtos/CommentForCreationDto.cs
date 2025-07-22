namespace CleanArchitecture.Application.DTOs.CommentDtos;

public record CommentForCreationDto(string Content, Guid TaskId);
