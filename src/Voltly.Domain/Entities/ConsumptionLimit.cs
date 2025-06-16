using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voltly.Domain.Entities;

[Table("TB_CONSUMPTION_LIMITS")]
public class ConsumptionLimit : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    public long EquipmentId { get; set; }

    public virtual Equipment Equipment { get; set; } = null!;

    public double LimitKwh { get; set; }
    public DateOnly ComputedAt { get; set; }
}