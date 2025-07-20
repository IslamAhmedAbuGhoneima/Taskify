public class TaskInProjectDto
{
    public Guid TaskId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public string Priority { get; set; } = null!;

    public DateTime? DueDate { get; set; }

    public string? AssignedToUserName { get; set; }

    public bool IsOverdue { get; set; }
}
