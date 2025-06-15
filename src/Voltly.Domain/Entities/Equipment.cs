using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voltly.Domain.Entities;

[Table("TB_EQUIPMENTS")]
public class Equipment : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]  public long  OwnerId { get; set; }
    public User Owner { get; set; } = null!;

    [Required, MaxLength(120)] public string Name { get; set; } = null!;
    [MaxLength(250)]           public string? Description { get; set; }
    public double              DailyLimitKwh { get; set; }
    public bool                Active        { get; set; } = true;

    public ICollection<Sensor> Sensors { get; set; } = new HashSet<Sensor>();
}