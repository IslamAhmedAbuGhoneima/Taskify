using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Application.Interfaces.Repositories;

public interface ICommentRepository : IBaseRepository<Comment>
{
    IEnumerable<Comment> GetCommentByTaskWithUser(Guid taskId);

    Comment? GetCommentWithUser(Guid? commentId);
}
