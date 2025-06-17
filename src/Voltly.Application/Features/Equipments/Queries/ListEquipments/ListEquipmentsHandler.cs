using MediatR;
using Microsoft.EntityFrameworkCore;  
using Voltly.Application.Abstractions;  
using Voltly.Application.DTOs;  
using Voltly.Application.DTOs.Equipments;  
using Voltly.Domain.Entities;  


namespace Voltly.Application.Features.Equipments.Queries.ListEquipments;

public sealed class ListEquipmentsHandler
    : IRequestHandler<ListEquipmentsQuery, PagedResponse<EquipmentResponse>>
{
    private readonly IEquipmentRepository _repo;
    private readonly IMapper _map;
    public ListEquipmentsHandler(IEquipmentRepository repo, IMapper map)
        => (_repo, _map) = (repo, map);

    public async Task<PagedResponse<EquipmentResponse>> Handle(
        ListEquipmentsQuery q, CancellationToken ct)
    {
        var query = _repo.Queryable()
            .Where(e => e.OwnerId == q.OwnerId)
            .OrderBy(e => e.Name);

        var total = await query.CountAsync(ct);
        var items = await query.Skip((q.Page - 1) * q.Size)
            .Take(q.Size)
            .ToListAsync(ct);

        return new PagedResponse<EquipmentResponse>(
            _map.MapCollection<Equipment, EquipmentResponse>(items).ToList(),
            total, q.Page, q.Size);
    }
}
