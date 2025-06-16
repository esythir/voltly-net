using System.ComponentModel.DataAnnotations;

namespace Voltly.Application.DTOs.Auth;

public sealed record LoginRequest(
    [Required, EmailAddress]            string Email,
    [Required, StringLength(50, MinimumLength = 6)]
    string Password);