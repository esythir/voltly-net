using Mapster;
using MediatR;
using Voltly.Application.Abstractions;
using Voltly.Application.DTOs.Equipments;
using Voltly.Domain.Entities;
using Voltly.Domain.Exceptions;

namespace Voltly.Application.Features.Equipments.Commands.UpdateEquipment;

public sealed class UpdateEquipmentHandler
    : IRequestHandler<UpdateEquipmentCommand, EquipmentResponse>
{
    private readonly IEquipmentRepository _repo;
    private readonly IUnitOfWork          _uow;
    private readonly IMapper              _map;

    public UpdateEquipmentHandler(
        IEquipmentRepository repo,
        IUnitOfWork          uow,
        IMapper              map)
        => (_repo, _uow, _map) = (repo, uow, map);

    public async Task<EquipmentResponse> Handle(UpdateEquipmentCommand cmd, CancellationToken ct)
    {
        // 1. Carrega entidade
        var eq = await _repo.GetAsync(cmd.Id, ct)
                 ?? throw new DomainException($"Equipment {cmd.Id} not found.");

        // 2. Garantir que o equipamento pertence ao usuário
        if (eq.OwnerId != cmd.OwnerId)
            throw new DomainException("Você não é o proprietário deste equipamento.");

        // 3. Verificar nome duplicado (caso tenha sido alterado)
        if (!eq.Name.Equals(cmd.Request.Name, StringComparison.OrdinalIgnoreCase) &&
            await _repo.ExistsByNameAsync(cmd.Request.Name, ct))
            throw new DomainException($"Nome '{cmd.Request.Name}' já está em uso.");

        // 4. Mapear alterações
        cmd.Request.Adapt(eq);
        _repo.Update(eq);

        // 5. Persistir
        await _uow.CommitAsync(ct);

        // 6. Retornar DTO
        return _map.Map<EquipmentResponse>(eq);
    }
}