using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voltly.Domain.Entities;

namespace Voltly.Infrastructure.Persistence.Configurations;

public class SensorConfiguration : IEntityTypeConfiguration<Sensor>
{
    public void Configure(EntityTypeBuilder<Sensor> b)
    {
        b.ToTable("TB_SENSORS");

        b.HasKey(s => s.Id);
        b.Property(s => s.SerialNumber)
            .IsRequired()
            .HasMaxLength(100);

        b.HasIndex(s => s.SerialNumber).IsUnique();

        b.HasOne(s => s.Equipment)
            .WithMany(e => e.Sensors)
            .HasForeignKey(s => s.EquipmentId);
    }
}
