using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.Models;

public class Comment
{
    public Guid Id { get; set; }

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("Task")]
    public Guid TaskId { get; set; } 
    public Task Task { get; set; } = null!;

    [ForeignKey("User")]
    public string AuthorId { get; set; } = null!;
    public User User { get; set; } = null!;
}
