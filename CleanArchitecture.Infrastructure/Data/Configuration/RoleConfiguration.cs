using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        IdentityRole[] roles = [
            new IdentityRole { Name = "admin", NormalizedName = "ADMIN" },
            new IdentityRole { Name = "member", NormalizedName = "MEMBER" },
        ];

        builder.HasData(roles);
    }
}
