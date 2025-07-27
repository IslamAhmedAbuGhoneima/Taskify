using CleanArchitecture.Application.DTOs.AttachmentDtos;
using System.Net.Mail;

namespace CleanArchitecture.Application.Interfaces.Services;

public interface IAttachmentService
{
    Task<AttachmentDto> UploadAttachment(AttachmentForCreationDto request);

    AttachmentDto GetAttachment(Guid attachmentId);

    IEnumerable<AttachmentDto> GetTaskAttachments(Guid taskId);

    Task<bool> DeleteAttachment(Guid attachmentId);
}
