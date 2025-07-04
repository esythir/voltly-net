using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Voltly.Application.Abstractions;
using Voltly.Application.DTOs;
using Voltly.Domain.Entities;

namespace Voltly.Application.Features.Reports.Queries.GetDailyCo2;

public sealed class GetDailyCo2Handler
    : IRequestHandler<GetDailyCo2Query, IEnumerable<DailyReportDto>>
{
    private readonly IRepository<DailyReport> _repo;
    private readonly IMapper _map;
    public GetDailyCo2Handler(IRepository<DailyReport> repo, IMapper map)
        => (_repo, _map) = (repo, map);

    public async Task<IEnumerable<DailyReportDto>> Handle(
        GetDailyCo2Query q, CancellationToken ct)
    {
        IQueryable<DailyReport> query = _repo.Queryable();

        if (q.EquipmentId is not null)
            query = query.Where(r => r.EquipmentId == q.EquipmentId);
        if (q.From is not null)
            query = query.Where(r => r.ReportDate >= q.From);
        if (q.To is not null)
            query = query.Where(r => r.ReportDate <= q.To);

        query = query.OrderByDescending(r => r.ReportDate);

        var list = await query.ToListAsync(ct);
        return _map.MapCollection<DailyReport, DailyReportDto>(list);
    }
}