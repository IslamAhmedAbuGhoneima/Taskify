using CleanArchitecture.Application.DTOs.CommentDtos;

namespace CleanArchitecture.Application.Interfaces.Services;

public interface ICommentService
{
    IEnumerable<CommentDto> GetCommentsByTask(Guid taskId);

    Task<CommentDto> CreateComment(CommentForCreationDto request);

    CommentDto GetComment(Guid commentId);

    Task<bool> DeleteComment(Guid commentId);

    Task<bool> UpdateComment(Guid commentId, CommentForUpdateDto request);
}
