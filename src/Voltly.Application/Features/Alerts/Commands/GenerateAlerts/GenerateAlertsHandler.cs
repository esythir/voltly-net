using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Voltly.Application.Abstractions;
using Voltly.Domain.Entities;

namespace Voltly.Application.Features.Alerts.Commands.GenerateAlerts;

public sealed class GenerateAlertsHandler : IRequestHandler<GenerateAlertsCommand, int>
{
    private readonly IRepository<DailyReport>      _reports;
    private readonly IRepository<ConsumptionLimit> _limits;
    private readonly IRepository<Alert>            _alerts;
    private readonly IUnitOfWork                   _uow;

    public GenerateAlertsHandler(
        IRepository<DailyReport>      reports,
        IRepository<ConsumptionLimit> limits,
        IRepository<Alert>            alerts,
        IUnitOfWork                   uow) =>
        (_reports, _limits, _alerts, _uow) = (reports, limits, alerts, uow);

    public async Task<int> Handle(GenerateAlertsCommand _, CancellationToken ct)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var reports = await _reports.Queryable()
                                    .Where(r => r.ReportDate == today)
                                    .ToListAsync(ct);

        var limits  = await _limits.Queryable()
                                   .Where(l => l.ComputedAt.Month == today.Month &&
                                               l.ComputedAt.Year  == today.Year)
                                   .ToDictionaryAsync(l => l.EquipmentId, ct);

        var alertsToSave = new List<Alert>();

        foreach (var rep in reports)
        {
            if (!limits.TryGetValue(rep.EquipmentId, out var limit)) continue;
            if (rep.ConsumptionKwh <= limit.LimitKwh)                 continue;

            alertsToSave.Add(new Alert
            {
                EquipmentId    = rep.EquipmentId,
                AlertDate      = today,
                ConsumptionKwh = rep.ConsumptionKwh,
                LimitKwh       = limit.LimitKwh,
                ExceededByKwh  = rep.ConsumptionKwh - limit.LimitKwh,
                Message        = $"Consumption exceeded by {rep.ConsumptionKwh - limit.LimitKwh:0.##} kWh"
            });
        }

        await _alerts.AddAsyncRange(alertsToSave, ct);
        await _uow.CommitAsync(ct);

        return alertsToSave.Count;
    }
}
