using Ardalis.GuardClauses;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Voltly.Application.Abstractions;
using Voltly.Application.DTOs;
using Voltly.Domain.Entities;

namespace Voltly.Application.Features.IdleActions.CheckIdle;

public sealed class CheckIdleHandler
    : IRequestHandler<CheckIdleQuery, IEnumerable<AutomaticActionDto>>
{
    private readonly IRepository<EnergyReading> _readRepo;
    private readonly IRepository<AutomaticAction> _actionRepo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _map;

    public CheckIdleHandler(
        IRepository<EnergyReading> readRepo,
        IRepository<AutomaticAction> actionRepo,
        IUnitOfWork uow,
        IMapper map)
        => (_readRepo, _actionRepo, _uow, _map) = (readRepo, actionRepo, uow, map);

    public async Task<IEnumerable<AutomaticActionDto>> Handle(
        CheckIdleQuery q, CancellationToken ct)
    {
        var since = DateTime.UtcNow.AddMinutes(-q.MinutesWindow);

        var sensorQ = _readRepo.Queryable()
            .Where(r => r.TakenAt >= since);

        if (q.EquipmentId is not null)
            sensorQ = sensorQ.Where(r => r.Sensor.EquipmentId == q.EquipmentId);

        var grouped = await sensorQ
            .GroupBy(r => r.Sensor.EquipmentId)
            .ToListAsync(ct);

        var actions = new List<AutomaticAction>();

        foreach (var g in grouped)
        {
            var avgPower = g.Average(r => r.PowerKw);
            var avgOcc   = g.Average(r => r.OccupancyPct);
            
            if (avgPower < 0.05 /*kW*/ && avgOcc < 10)
            {
                actions.Add(new AutomaticAction
                {
                    EquipmentId = g.Key,
                    Type   = "SHUTDOWN",
                    Details = $"Automatic shutdown – Average power {avgPower:0.000} kW " +
                              $" and occupancy {avgOcc:0.#} %",
                    ExecutedAt = DateTime.UtcNow
                });
            }
        }

        await _actionRepo.AddAsyncRange(actions, ct);
        await _uow.CommitAsync(ct);

        return _map.MapCollection<AutomaticAction, AutomaticActionDto>(actions);
    }
}
