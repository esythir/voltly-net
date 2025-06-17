using Microsoft.EntityFrameworkCore;
using Voltly.Domain.Entities;

namespace Voltly.Application.Abstractions;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Equipment> Equipments { get; }
    DbSet<Sensor> Sensors { get; }
    DbSet<EnergyReading> EnergyReadings { get; }
    DbSet<ConsumptionLimit> ConsumptionLimits { get; }
    DbSet<AutomaticAction> AutomaticActions { get; }
    DbSet<Alert> Alerts { get; }
    DbSet<DailyReport> DailyReports { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}