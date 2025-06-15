using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Voltly.Domain.Entities;

namespace Voltly.Infrastructure.Persistence.Configurations;

public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> b)
    {
        b.ToTable("TB_EQUIPMENTS");

        b.HasKey(e => e.Id);

        b.Property(e => e.Name).IsRequired().HasMaxLength(120);
        b.HasIndex(e => new { e.OwnerId, e.Name }).IsUnique();

        // Relação 1-N User→Equipments
        b.HasOne(e => e.Owner)
            .WithMany(u => u.Equipments)
            .HasForeignKey(e => e.OwnerId);

        b.Property(e => e.Active)
            .HasDefaultValue(true);
    }
}
