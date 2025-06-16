using MediatR;
using Microsoft.EntityFrameworkCore;
using Voltly.Application.Abstractions;
using Voltly.Domain.Entities;

namespace Voltly.Application.Features.Limits.Commands.RecalculateMonthly;

public sealed class RecalculateMonthlyHandler : IRequestHandler<RecalculateMonthlyCommand>
{
    private readonly IRepository<ConsumptionLimit> _repo;
    private readonly IRepository<DailyReport>      _daily;
    private readonly IUnitOfWork                   _uow;

    public RecalculateMonthlyHandler(
        IRepository<ConsumptionLimit> repo,
        IRepository<DailyReport>      daily,
        IUnitOfWork                   uow) =>
        (_repo, _daily, _uow) = (repo, daily, uow);

    public async Task Handle(RecalculateMonthlyCommand c, CancellationToken ct)
    {
        var ym    = c.YearMonth ?? int.Parse(DateTime.UtcNow.ToString("yyyyMM"));
        var year  = ym / 100;
        var month = ym % 100;

        var monthReports = _daily.Queryable()
                                 .Where(r => r.ReportDate.Year  == year &&
                                             r.ReportDate.Month == month);

        var grouped = await monthReports
            .GroupBy(r => r.EquipmentId)
            .Select(g => new { Equip = g.Key, Avg = g.Average(r => r.ConsumptionKwh) })
            .ToListAsync(ct);

        foreach (var g in grouped)
        {
            var firstDay = new DateOnly(year, month, 1);
            var limit = await _repo.Queryable(false)
                                   .FirstOrDefaultAsync(l => l.EquipmentId == g.Equip &&
                                                             l.ComputedAt  == firstDay, ct);

            if (limit is null)
            {
                limit = new ConsumptionLimit { EquipmentId = g.Equip };
                await _repo.AddAsync(limit, ct);
            }

            limit.LimitKwh  = g.Avg * 1.10;   // +10 %
            limit.ComputedAt = firstDay;
            _repo.Update(limit);
        }

        await _uow.CommitAsync(ct);
    }
}
