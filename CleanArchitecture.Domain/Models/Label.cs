using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.Models;

public class Label
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Color { get; set; } = "#000000";

    public DateTime CreatedAt { get; set; }

    [ForeignKey("Workspace")]
    public Guid WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = null!;
}
