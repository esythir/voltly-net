using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Voltly.Domain.Entities;

namespace Voltly.Infrastructure.Persistence.Configurations;

public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> b)
    {
        b.ToTable("TB_EQUIPMENTS");

        b.HasKey(e => e.Id);

        b.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(120);

        b.HasIndex(e => new { e.OwnerId, e.Name })
            .IsUnique();

        b.HasOne(e => e.Owner)
            .WithMany(u => u.Equipments)
            .HasForeignKey(e => e.OwnerId);

        /**  👇  bool → NUMBER(1)  **/
        b.Property(e => e.Active)
            .HasConversion<BoolToZeroOneConverter<int>>()
            // ► escolha SÓ UMA das opções abaixo ◄
            //.HasDefaultValueSql("1")      // default 1 no Oracle
            //.HasDefaultValueSql("0")      // se quiser inativo
            /* ou simplesmente SEM default e você define em código */
            ;
    }
}