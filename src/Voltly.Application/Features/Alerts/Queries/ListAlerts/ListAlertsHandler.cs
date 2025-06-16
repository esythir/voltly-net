using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Voltly.Application.Abstractions;
using Voltly.Application.DTOs;
using Voltly.Domain.Entities;

namespace Voltly.Application.Features.Alerts.Queries.ListAlerts;

public sealed class ListAlertsHandler
    : IRequestHandler<ListAlertsQuery, PagedResponse<AlertDto>>
{
    private readonly IRepository<Alert> _repo;
    public ListAlertsHandler(IRepository<Alert> repo) => _repo = repo;

    public async Task<PagedResponse<AlertDto>> Handle(
        ListAlertsQuery request,
        CancellationToken ct)
    {
        var query = _repo.Query();

        if (request.EquipmentId is not null)
            query = query.Where(a => a.EquipmentId == request.EquipmentId);

        query = request.Sort?.ToLowerInvariant() switch
        {
            "desc" => query.OrderByDescending(a => a.AlertDate),
            _      => query.OrderBy(a => a.AlertDate)
        };

        var total  = await query.CountAsync(ct);
        var alerts = await query.Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToListAsync(ct);

        var items = alerts.Select(a => a.Adapt<AlertDto>()).ToList();
        return new PagedResponse<AlertDto>(items, total, request.Page, request.Size);
    }
}