using MediatR;
using Voltly.Application.DTOs.Users;

namespace Voltly.Application.Features.Users.Commands.UpdateUserAdmin;

public sealed record UpdateUserAdminCommand(long Id, UpdateUserAdminRequest Request)
    : IRequest<UserResponse>;