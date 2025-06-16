using Voltly.Application.DTOs.Users;

namespace Voltly.Application.DTOs.Auth;

/// <summary>DTO devolvido no login (token + dados básicos do usuário)</summary>
public sealed record AuthResponse(
    string      Token,
    DateTime    ExpiresAt,
    UserResponse User);