using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voltly.Domain.Entities;

[Table("TB_SENSORS")]
public class Sensor : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; init; }

    [Required, MaxLength(100)] public string SerialNumber { get; set; } = null!;
    [Required, MaxLength(80)]  public string Type         { get; set; } = null!;

    [Required]  public long   EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = null!;
}