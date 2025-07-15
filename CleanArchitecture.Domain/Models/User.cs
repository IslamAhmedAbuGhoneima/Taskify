using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Domain.Models;

public class User : IdentityUser
{
    public string? PhotoURL { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime LastLoginAt { get; set; }

    public bool IsActive { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiryTime { get; set; }

    public ICollection<Workspace> OwnedWorkspaces { get; set; } = [];

    public ICollection<Notification> Notifications { get; set; } = [];

    public ICollection<Comment> Comments { get; set; } = [];

    public ICollection<Attachment> Attachments { get; set; } = [];

    public ICollection<UserWorkspace> UserWorkspaces { get; set; } = [];

    public ICollection<Task> Tasks { get; set; } = [];
}
