using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Application.Interfaces.Repositories;

public interface IAttachmentRepository : IBaseRepository<Attachment>
{
    IEnumerable<Attachment> GetTaskAttachments(Guid taskId);
}
