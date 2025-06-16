using MediatR;
using Voltly.Application.DTOs.Users;

namespace Voltly.Application.Features.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(long Id, UpdateUserRequest Request) : IRequest<UserResponse>;