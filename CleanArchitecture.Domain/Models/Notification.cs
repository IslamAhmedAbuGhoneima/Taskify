using CleanArchitecture.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.Models;

public class Notification
{
    public Guid Id { get; set; }

    public string Message { get; set; } = null!;

    public TaskType Type { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;

    [ForeignKey("Task")]
    public Guid? TargetTaskId {  get; set; }
    public Task? Task { get; set; }
}
