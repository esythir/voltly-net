namespace Voltly.Application.DTOs.Users;

public sealed record UserResponse(
    long   Id,
    string Name,
    string Email,
    DateOnly BirthDate,
    bool   IsActive,
    string Role,
    DateTime CreatedAt,
    DateTime? UpdatedAt);