using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.Models;

public class Workspace
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Slug { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }


    [ForeignKey("Owner")]
    public string OwnerId { get; set; } = null!;
    public User Owner { get; set; } = null!;

    public ICollection<UserWorkspace> Members { get; set; } = [];

    public ICollection<Project> Projects { get; set; } = [];

    public ICollection<Label> Labels { get; set; } = [];

}
