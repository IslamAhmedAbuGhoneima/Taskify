using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Domain.Models;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories;

public class CommentRepository : BaseRepository<Comment>, ICommentRepository
{
    readonly TaskifyDbContext _context;

    public CommentRepository(TaskifyDbContext context) 
        : base(context) => _context = context;


    public IEnumerable<Comment> GetCommentByTaskWithUser(Guid taskId)
       => _context.Comments
            .Include(comment => comment.User)
            .Where(comment => comment.TaskId == taskId);

    public Comment? GetCommentWithUser(Guid? commentId)
        => _context.Comments.Include(comment => comment.User)
        .FirstOrDefault(comment => comment.Id == commentId);
}
