using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Voltly.Application.Abstractions;
using Voltly.Application.DTOs;
using Voltly.Domain.Entities;

namespace Voltly.Application.Features.Reports.Queries.GetDailyHistory;

public sealed class GetDailyHistoryHandler
    : IRequestHandler<GetDailyHistoryQuery, PagedResponse<DailyReportDto>>
{
    private readonly IRepository<DailyReport> _repo;
    private readonly IMapper _map;
    public GetDailyHistoryHandler(IRepository<DailyReport> repo, IMapper map)
        => (_repo, _map) = (repo, map);

    public async Task<PagedResponse<DailyReportDto>> Handle(
        GetDailyHistoryQuery q, CancellationToken ct)
    {
        IQueryable<DailyReport> query = _repo.Queryable();

        if (q.EquipmentId is not null)
            query = query.Where(r => r.EquipmentId == q.EquipmentId);
        if (q.From is not null)
            query = query.Where(r => r.ReportDate >= q.From);
        if (q.To is not null)
            query = query.Where(r => r.ReportDate <= q.To);

        query = query.OrderByDescending(r => r.ReportDate);

        var total   = await query.CountAsync(ct);
        var reports = await query.Skip((q.Page - 1) * q.Size)
            .Take(q.Size)
            .ToListAsync(ct);

        var dto = _map.MapCollection<DailyReport, DailyReportDto>(reports).ToList();
        return new PagedResponse<DailyReportDto>(dto, total, q.Page, q.Size);
    }
}