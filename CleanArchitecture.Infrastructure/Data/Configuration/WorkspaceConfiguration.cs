using CleanArchitecture.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configuration;

public class WorkspaceConfiguration : IEntityTypeConfiguration<Workspace>
{
    public void Configure(EntityTypeBuilder<Workspace> builder)
    {
        builder.Property(prop => prop.Name)
            .HasMaxLength(120);

        builder.Property(prop => prop.Slug)
            .HasMaxLength(120);

        builder.HasIndex(prop => prop.Slug)
            .IsUnique();
    }
}
