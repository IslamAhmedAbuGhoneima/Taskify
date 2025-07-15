using CleanArchitecture.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data;

public class TaskifyDbContext : IdentityDbContext<User>
{
    public TaskifyDbContext(DbContextOptions<TaskifyDbContext> options)
        : base(options) { }
    

    public DbSet<Workspace> Workspaces {  get; set; }

    public DbSet<UserWorkspace> UserWorkspaces {  get; set; }

    public DbSet<Project> Projects { get; set; }

    public DbSet<Domain.Models.Task> Tasks {  get; set; }

    public DbSet<Label> Labels {  get; set; }

    public DbSet<TaskLabel> TaskLabels {  get; set; }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<Attachment> Attachments { get; set; }

    public DbSet<Notification> Notifications { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(TaskifyDbContext).Assembly);
        base.OnModelCreating(builder);
    }
}
