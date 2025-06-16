using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Voltly.Domain.Enums;

namespace Voltly.Domain.Entities;

[Table("TB_DAILY_REPORTS")]
public class DailyReport : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    public long EquipmentId { get; set; }

    public virtual Equipment Equipment { get; set; } = null!;

    public DateOnly ReportDate { get; set; }
    public double ConsumptionKwh { get; set; }
    public double Co2EmissionKg { get; set; }
    public EfficiencyRating EfficiencyRating { get; set; }

    public DateTime CreatedAt { get; private set; }
    public void OnCreate() => CreatedAt = DateTime.UtcNow;
}