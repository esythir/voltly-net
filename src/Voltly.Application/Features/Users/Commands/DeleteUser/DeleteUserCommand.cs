using MediatR;

namespace Voltly.Application.Features.Users.Commands.DeleteUser;

public sealed record DeleteUserCommand(long Id) : IRequest;