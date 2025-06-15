using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voltly.Domain.Entities;

[Table("TB_ALERTS")]
public class Alert : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required] public long EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = null!;

    public DateOnly AlertDate       { get; set; }
    public double   ConsumptionKwh  { get; set; }
    public double   LimitKwh        { get; set; }
    public double   ExceededByKwh   { get; set; }
    [MaxLength(500)] public string? Message { get; set; }

    public DateTime CreatedAt { get; private set; }
    public void OnCreate() => CreatedAt = DateTime.UtcNow;
}