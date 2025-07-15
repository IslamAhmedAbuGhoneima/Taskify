using CleanArchitecture.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configuration;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.Property(prop => prop.FileName)
            .HasMaxLength(260);


        builder.Property(prop => prop.ContentType)
            .HasMaxLength(100);
    }
}
