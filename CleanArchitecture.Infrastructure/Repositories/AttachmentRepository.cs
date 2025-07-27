using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Domain.Models;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories;

public class AttachmentRepository : BaseRepository<Attachment>, IAttachmentRepository
{
    readonly TaskifyDbContext _context;

    public AttachmentRepository(TaskifyDbContext context)
        : base(context) => _context = context;

    public IEnumerable<Attachment> GetTaskAttachments(Guid taskId)
        => _context.Attachments.AsNoTracking()
        .Where(attachment => attachment.TaskId == taskId);
}
