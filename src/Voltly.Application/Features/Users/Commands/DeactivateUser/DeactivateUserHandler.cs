using MediatR;
using Voltly.Application.Abstractions;
using Voltly.Domain.Exceptions;

namespace Voltly.Application.Features.Users.Commands.DeactivateUser;

public sealed class DeactivateUserHandler : IRequestHandler<DeactivateUserCommand>
{
    private readonly IUserRepository _repo;
    private readonly IUnitOfWork     _uow;

    public DeactivateUserHandler(IUserRepository repo, IUnitOfWork uow) =>
        (_repo, _uow) = (repo, uow);

    public async Task Handle(DeactivateUserCommand cmd, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(cmd.UserId, ct)
                   ?? throw new DomainException($"User {cmd.UserId} not found.");
        user.IsActive = false;
        _repo.Update(user);
        await _uow.CommitAsync(ct);
    }
}