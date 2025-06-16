using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Voltly.Domain.Enums;

namespace Voltly.Domain.Entities;

[Table("TB_USERS")]
public class User : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required, MaxLength(120)]
    public string Name { get; set; } = null!;

    [Required, EmailAddress, MaxLength(180)]
    public string Email { get; set; } = null!;

    [Required, MaxLength(100)]
    public string Password { get; set; } = null!;

    public DateOnly BirthDate { get; set; }
    public UserRole Role { get; set; } = UserRole.User;

    [Column(TypeName = "NUMBER(1)")]
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public virtual ICollection<Equipment> Equipments { get; set; } = new HashSet<Equipment>();

    public void OnCreate()
    {
        var now = DateTime.UtcNow;
        CreatedAt = now;
        UpdatedAt = now;
    }
    public void OnUpdate() => UpdatedAt = DateTime.UtcNow;
}