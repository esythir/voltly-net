using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voltly.Domain.Entities;

namespace Voltly.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("TB_USERS");

        b.HasKey(x => x.Id);
        b.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(120);

        b.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(180);

        b.HasIndex(x => x.Email).IsUnique();

        b.Property(x => x.Password).IsRequired().HasMaxLength(100);

        b.Property(x => x.Role)
            .HasConversion<string>()
            .HasMaxLength(30);

        // Auditing
        b.Property(x => x.CreatedAt).HasDefaultValueSql("SYSTIMESTAMP");
    }
}