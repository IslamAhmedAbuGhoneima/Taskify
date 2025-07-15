using CleanArchitecture.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configuration;

public class LableConfiguration : IEntityTypeConfiguration<Label>
{
    public void Configure(EntityTypeBuilder<Label> builder)
    {
        builder.Property(prop => prop.Name)
            .HasMaxLength(60);

        builder.Property(prop => prop.Color)
            .HasMaxLength(7);

        builder.HasIndex(l => new { l.WorkspaceId, l.Name })
            .IsUnique();
    }
}
