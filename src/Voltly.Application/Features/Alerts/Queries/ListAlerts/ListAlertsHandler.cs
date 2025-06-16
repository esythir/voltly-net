using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Voltly.Infrastructure.Repositories;
using Voltly.Application.Abstractions;
using Voltly.Domain.Entities;
using Voltly.Application.DTOs;

namespace Voltly.Application.Features.Alerts.Queries.ListAlerts;

public sealed class ListAlertsHandler
    : IRequestHandler<ListAlertsQuery, PagedResponse<AlertDto>>
{
    private readonly IRepository<Alert> _repo;

    public ListAlertsHandler(IRepository<Alert> repo) => _repo = repo;

    public async Task<PagedResponse<AlertDto>> Handle(
        ListAlertsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _repo.Query();

        if (request.EquipmentId.HasValue)
            query = query.Where(a => a.EquipmentId == request.EquipmentId);

        // ordenação dinâmica
        query = request.Sort?.ToLowerInvariant() switch
        {
            "desc" => query.OrderByDescending(a => a.AlertDate),
            _      => query.OrderBy(a => a.AlertDate)
        };

        // paginação
        var paged = await query.ToPagedAsync(
            request.PageNumber, request.PageSize, cancellationToken);

        // mapeia resultado
        return paged.Map(a => a.Adapt<AlertDto>());
    }
}