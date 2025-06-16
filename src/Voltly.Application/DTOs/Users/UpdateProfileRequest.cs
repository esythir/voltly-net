using System.ComponentModel.DataAnnotations;

namespace Voltly.Application.DTOs.Users;

/// <summary>Fields that a NORMAL user can change.</summary>
public sealed record UpdateProfileRequest(
    [Required, StringLength(120, MinimumLength = 2)]  string Name,
    [Required, EmailAddress]                          string Email,
    [StringLength(50, MinimumLength = 6)]             string? Password,
    DateOnly?                                         BirthDate);