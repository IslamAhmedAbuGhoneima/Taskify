namespace CleanArchitecture.Application.DTOs.CommentDtos;

public record CommentDto(Guid Id, string Content, string UserName ,DateTime CreatedAt, DateTime? UpdatedAt);
