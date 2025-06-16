using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voltly.Domain.Entities;

[Table("TB_ENERGY_READINGS")]
public class EnergyReading : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    public long SensorId { get; set; }

    public virtual Sensor Sensor { get; set; } = null!;

    public double PowerKw { get; set; }
    public double OccupancyPct { get; set; }
    public DateTime TakenAt { get; set; }
}