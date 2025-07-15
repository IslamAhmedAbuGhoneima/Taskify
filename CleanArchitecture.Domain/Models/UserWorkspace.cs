using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.Models;

[PrimaryKey(nameof(UserId),nameof(WorkspaceId))]
public class UserWorkspace
{
    [ForeignKey("User")]
    public string UserId { get; set; } = null!;
    public User User { get; set; }

    [ForeignKey("Workspace")]
    public Guid WorkspaceId { get; set; } 
    public Workspace Workspace { get; set; }

    public string Role { get; set; } = "Member";

    public DateTime JoinedAt { get; set; }
}
