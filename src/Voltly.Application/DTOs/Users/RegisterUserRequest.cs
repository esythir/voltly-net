using System.ComponentModel.DataAnnotations;

namespace Voltly.Application.DTOs.Users;

public sealed record RegisterUserRequest(
    [Required, StringLength(120, MinimumLength = 2)] string Name,
    [Required, EmailAddress]                        string Email,
    [Required, StringLength(50, MinimumLength = 6)] string Password,
    [Required]                                      DateOnly BirthDate);