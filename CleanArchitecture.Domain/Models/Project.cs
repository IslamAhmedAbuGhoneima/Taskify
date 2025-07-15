using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.Models;

public class Project
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Color { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ArchivedAt { get; set; }

    [ForeignKey("Workspace")]
    public Guid WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = null!;

    public ICollection<Task> Tasks { get; set; } = [];
}
