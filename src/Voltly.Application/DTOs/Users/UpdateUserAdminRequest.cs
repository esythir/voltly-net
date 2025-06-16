using System.ComponentModel.DataAnnotations;

namespace Voltly.Application.DTOs.Users;

/// <summary>Fields that an ADMIN user can change.</summary>
public sealed record UpdateUserAdminRequest(
    [Required, StringLength(120, MinimumLength = 2)]  string Name,
    [Required, EmailAddress]                          string Email,
    [StringLength(50, MinimumLength = 6)]             string? Password,
    DateOnly?                                         BirthDate,
    [Required]                                        string Role,
    bool                                              IsActive);