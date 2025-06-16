using MediatR;

namespace Voltly.Application.Features.Users.Commands.DeactivateUser;

public sealed record DeactivateUserCommand(long UserId) : IRequest;