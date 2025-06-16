using Mapster;
using MediatR;
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
        var query = _repo.Queryable()
            .OrderByDescending(r => r.ReportDate);

        if (q.EquipmentId is not null)
            query = query.Where(r => r.EquipmentId == q.EquipmentId);

        if (q.From is not null)
            query = query.Where(r => r.ReportDate >= q.From);

        if (q.To is not null)
            query = query.Where(r => r.ReportDate <= q.To);

        var paged = await query.ToPagedAsync(q.Page, q.Size, ct);
        var dto   = _map.MapCollection<DailyReport, DailyReportDto>(paged.Data);

        return new(dto.ToList(), paged.Total, q.Page, q.Size);
    }
}