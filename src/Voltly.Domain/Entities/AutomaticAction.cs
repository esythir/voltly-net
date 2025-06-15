using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Voltly.Domain.Entities;

[Table("TB_AUTOMATIC_ACTIONS")]
public class AutomaticAction : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]  public long EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = null!;

    [Required, MaxLength(40)]  public string Type    { get; set; } = null!;
    [Required, MaxLength(500)] public string Details { get; set; } = null!;
    public DateTime ExecutedAt { get; set; }
}