using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Voltly.Domain.Enums;

namespace Voltly.Domain.Entities;

[Table("TB_USERS")]
public class User : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; init; }

    [Required, MaxLength(120)]                public string Name     { get; set; } = null!;
    [Required, EmailAddress, MaxLength(180)]  public string Email    { get; set; } = null!;
    [Required, MaxLength(100)]                public string Password { get; set; } = null!;
                                              
    public DateOnly BirthDate { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
    public bool     IsActive { get; set; } = true;

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt{ get; private set; }

    public ICollection<Equipment> Equipments { get; init; } = new HashSet<Equipment>();

    /* hooks */
    public void OnCreate() => CreatedAt = UpdatedAt = DateTime.UtcNow;
    public void OnUpdate() => UpdatedAt = DateTime.UtcNow;
}