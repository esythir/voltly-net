namespace Voltly.Application.DTOs;

public record UserDto(long Id, string Name, string Email, string Role, bool IsActive);