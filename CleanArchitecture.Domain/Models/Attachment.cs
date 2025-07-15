using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.Models;

public class Attachment
{
    public Guid Id { get; set; }

    public string FileName { get; set; } = null!;

    public string BlobPath { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public long SizeBytes { get; set; }

    public DateTime UploadedAt { get; set; }

    [ForeignKey("Task")]
    public Guid TaskId { get; set; }
    public Task Task { get; set; } = null!;

    [ForeignKey("UploadedBy")]
    public string UploadedByUserId { get; set; } = null!;
    public User UploadedBy { get; set; } = null!;

}
