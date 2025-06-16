using MediatR;
using Voltly.Application.Abstractions;
using Voltly.Domain.Exceptions;

namespace Voltly.Application.Features.Users.Commands.DeleteUser;

public sealed class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository _repo;
    private readonly IUnitOfWork     _uow;
    public DeleteUserHandler(IUserRepository repo, IUnitOfWork uow) =>
        (_repo, _uow) = (repo, uow);

    public async Task Handle(DeleteUserCommand cmd, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(cmd.Id, ct)
                   ?? throw new DomainException($"User {cmd.Id} not found.");

        await _repo.DeleteAsync(user, ct);
        await _uow.CommitAsync(ct);
    }
}