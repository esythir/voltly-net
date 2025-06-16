using MediatR;
using Voltly.Application.DTOs.Users;

namespace Voltly.Application.Features.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(RegisterUserRequest Request) : IRequest<UserResponse>;