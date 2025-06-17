using Mapster;
using MediatR;
using Voltly.Application.Abstractions;
using Voltly.Application.DTOs.Equipments;
using Voltly.Domain.Entities;
using Voltly.Domain.Exceptions;


namespace Voltly.Application.Features.Equipments.Commands.CreateEquipment;

public sealed class CreateEquipmentHandler
    : IRequestHandler<CreateEquipmentCommand, EquipmentResponse>
{
    private readonly IEquipmentRepository _repo;
    private readonly IUnitOfWork          _uow;
    private readonly IMapper              _map;

    public CreateEquipmentHandler(IEquipmentRepository repo, IUnitOfWork uow, IMapper map)
        => (_repo, _uow, _map) = (repo, uow, map);

    public async Task<EquipmentResponse> Handle(CreateEquipmentCommand c, CancellationToken ct)
    {
        if (await _repo.ExistsByNameAsync(c.Request.Name, ct))
            throw new DomainException($"Equipment '{c.Request.Name}' already exists.");

        var entity = c.Request.Adapt<Equipment>();
        entity.OwnerId = c.OwnerId;

        await _repo.AddAsync(entity, ct);
        await _uow.CommitAsync(ct);

        return _map.Map<EquipmentResponse>(entity);
    }
}
