using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;   // 👈 novo
using Voltly.Domain.Entities;
using Voltly.Infrastructure.Persistence.Converters;

namespace Voltly.Infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("TB_USERS");

        b.HasKey(u => u.Id);
        b.Property(u => u.Id).UseIdentityColumn();

        b.Property(u => u.Name) .IsRequired().HasMaxLength(120);
        b.Property(u => u.Email).IsRequired().HasMaxLength(180);
        b.HasIndex (u => u.Email).IsUnique();

        b.Property(u => u.Password).IsRequired().HasMaxLength(100);

        b.Property(u => u.BirthDate)
            .HasConversion<DateOnlyConverter, DateOnlyComparer>();

        b.Property(u => u.Role)
            .HasConversion<string>()
            .HasMaxLength(30);

        /* 👇 bool → NUMBER(1) + default 1 */
        b.Property(u => u.IsActive)
            .HasConversion<BoolToZeroOneConverter<int>>()
            //.HasDefaultValue(1);
            ;

        b.Property(u => u.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("SYSTIMESTAMP");

        b.Property(u => u.UpdatedAt)
            .ValueGeneratedOnAddOrUpdate();
    }
}