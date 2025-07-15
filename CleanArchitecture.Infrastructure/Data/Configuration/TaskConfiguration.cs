using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace CleanArchitecture.Infrastructure.Data.Configuration;

public class TaskConfiguration : IEntityTypeConfiguration<Domain.Models.Task>
{
    public void Configure(EntityTypeBuilder<Domain.Models.Task> builder)
    {
        builder.Property(prop => prop.Title)
            .HasMaxLength(200);

        builder.Property(prop => prop.Status)
            .HasConversion<string>();

        builder.Property(prop => prop.Priority)
            .HasConversion<string>();

        builder.HasOne(prop => prop.User)
            .WithMany(prop => prop.Tasks)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
