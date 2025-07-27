namespace CleanArchitecture.Application.DTOs.AttachmentDtos;

public record AttachmentDto(Guid Id,
    string FileName,
    string BlobPath,
    string ContentType,
    long SizeBytes,
    DateTime UploadedAt,
    Guid TaskId,
    string UploadedByUserId);
