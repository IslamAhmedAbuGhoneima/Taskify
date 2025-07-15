using CleanArchitecture.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace CleanArchitecture.Infrastructure.Data.Configuration;

public class UserWorkspaceConfiguration : IEntityTypeConfiguration<UserWorkspace>
{
    public void Configure(EntityTypeBuilder<UserWorkspace> builder)
    {
        builder.Property(prop => prop.Role)
        .HasMaxLength(20);

        builder.HasOne(uw => uw.User)
            .WithMany(u => u.UserWorkspaces)
            .HasForeignKey(uw => uw.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(uw => uw.Workspace)
            .WithMany(w => w.Members)
            .HasForeignKey(uw => uw.WorkspaceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
