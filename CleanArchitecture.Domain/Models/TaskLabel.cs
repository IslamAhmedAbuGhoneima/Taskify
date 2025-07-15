using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.Models;

[PrimaryKey(nameof(TaskId),nameof(LableId))]
public class TaskLabel
{
    [ForeignKey("Task")]
    public Guid TaskId { get; set; }
    public Task Task { get; set; } = null!;

    [ForeignKey( "Label")]
    public Guid LableId { get; set; }
    public Label Label { get; set; } = null!;
}
