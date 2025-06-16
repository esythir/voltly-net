using Ardalis.GuardClauses;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Voltly.Application.Abstractions;
using Voltly.Application.DTOs;
using Voltly.Domain.Entities;
using Voltly.Domain.Enums;

namespace Voltly.Application.Features.Reports.Commands.GenerateDaily;

public sealed class GenerateDailyReportHandler
    : IRequestHandler<GenerateDailyReportCommand, DailyReportDto>
{
    private readonly IRepository<EnergyReading> _reads;
    private readonly IRepository<DailyReport>   _repo;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _map;

    public GenerateDailyReportHandler(
        IRepository<EnergyReading> reads,
        IRepository<DailyReport> repo,
        IUnitOfWork uow,
        IMapper map) =>
        (_reads, _repo, _uow, _map) = (reads, repo, uow, map);

    public async Task<DailyReportDto> Handle(GenerateDailyReportCommand c, CancellationToken ct)
    {
        var day = c.ReportDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var (start, end) = (day.ToDateTime(TimeOnly.MinValue),
                            day.ToDateTime(TimeOnly.MaxValue));

        var consumption = await _reads.Queryable(false)
            .Where(r => r.Sensor.EquipmentId == c.EquipmentId &&
                        r.TakenAt >= start && r.TakenAt <= end)
            .SumAsync(r => r.PowerKw / 60, ct);

        var rating = consumption switch
        {
            < 2  => EfficiencyRating.Good,
            <= 5 => EfficiencyRating.Average,
            _    => EfficiencyRating.Poor
        };

        var entity = new DailyReport
        {
            EquipmentId      = c.EquipmentId,
            ReportDate       = day,
            ConsumptionKwh   = consumption,
            Co2EmissionKg    = consumption * 0.0006,
            EfficiencyRating = rating
        };
        entity.OnCreate();

        await _repo.AddAsync(entity, ct);
        await _uow.CommitAsync(ct);

        return _map.Map<DailyReportDto>(entity);
    }
}
