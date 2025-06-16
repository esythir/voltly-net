using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voltly.Domain.Entities;

namespace Voltly.Infrastructure.Persistence.Configurations;

public sealed class AlertConfiguration : IEntityTypeConfiguration<Alert>
{
    public void Configure(EntityTypeBuilder<Alert> b)
    {
        b.ToTable("TB_ALERTS");
        b.HasKey(a => a.Id);

        b.Property(a => a.Message).HasMaxLength(500);
        b.Property(a => a.CreatedAt)
            .HasDefaultValueSql("SYSTIMESTAMP");

        b.HasOne(a => a.Equipment)
            .WithMany()
            .HasForeignKey(a => a.EquipmentId);
    }
}