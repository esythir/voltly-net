using MediatR;
using Voltly.Application.DTOs;

namespace Voltly.Application.Features.Users.Commands.CreateUser;

public sealed record CreateUserCommand(
    string Name,
    string Email,
    string Password,
    DateOnly BirthDate) : IRequest<UserDto>;