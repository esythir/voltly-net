using MediatR;
using Voltly.Application.Abstractions;
using Voltly.Domain.Exceptions;

namespace Voltly.Application.Features.Equipments.Commands.ActivateEquipment;

public sealed class ActivateEquipmentHandler
    : IRequestHandler<ActivateEquipmentCommand>
{
    private readonly IEquipmentRepository _repo;
    private readonly IUnitOfWork          _uow;

    public ActivateEquipmentHandler(IEquipmentRepository repo, IUnitOfWork uow)
        => (_repo, _uow) = (repo, uow);

    public async Task Handle(ActivateEquipmentCommand cmd, CancellationToken ct)
    {
        var eq = await _repo.GetAsync(cmd.Id, ct)
                 ?? throw new DomainException($"Equipment {cmd.Id} not found.");

        if (eq.OwnerId != cmd.OwnerId)
            throw new DomainException("You are not the owner of this equipment.");

        eq.Active = true;
        _repo.Update(eq);
        await _uow.CommitAsync(ct);
    }
}