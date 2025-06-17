using MediatR;
using Voltly.Application.Abstractions;
using Voltly.Domain.Exceptions;

namespace Voltly.Application.Features.Equipments.Commands.DeactivateEquipment;

public sealed class DeactivateEquipmentHandler
    : IRequestHandler<DeactivateEquipmentCommand>
{
    private readonly IEquipmentRepository _repo;
    private readonly IUnitOfWork          _uow;

    public DeactivateEquipmentHandler(IEquipmentRepository repo, IUnitOfWork uow)
        => (_repo, _uow) = (repo, uow);

    public async Task Handle(DeactivateEquipmentCommand cmd, CancellationToken ct)
    {
        var eq = await _repo.GetAsync(cmd.Id, ct)
                 ?? throw new DomainException($"Equipment {cmd.Id} not found.");

        if (eq.OwnerId != cmd.OwnerId)
            throw new DomainException("You are not the owner of this equipment.");

        eq.Active = false;
        _repo.Update(eq);
        await _uow.CommitAsync(ct);
    }
}