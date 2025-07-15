using CleanArchitecture.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.Models;

public class Task
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public Status Status { get; set; }

    public Priority Priority { get; set; }

    public DateTime? DueDate { get; set; }

    public int OrderIndex { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [ForeignKey("Project")]
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    [ForeignKey("User")]
    public string? AssignedToUserId { get; set; } 
    public User? User { get; set; } 

    public ICollection<Comment> Comments { get; set; } = [];

    public ICollection<Attachment> Attachments { get; set; } = [];

    public ICollection<TaskLabel> TaskLabels { get; set; } = [];

    public ICollection<Notification> Notifications { get; set; } = [];
}
