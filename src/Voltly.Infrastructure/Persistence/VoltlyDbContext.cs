using Microsoft.EntityFrameworkCore;
using Voltly.Domain.Entities;
using Voltly.Application.Abstractions;

namespace Voltly.Infrastructure.Persistence;

public sealed class VoltlyDbContext : DbContext, IUnitOfWork
{
    public VoltlyDbContext(DbContextOptions<VoltlyDbContext> opt) : base(opt) { }

    public DbSet<User>             Users             => Set<User>();
    public DbSet<Equipment>        Equipments        => Set<Equipment>();
    public DbSet<Sensor>           Sensors           => Set<Sensor>();
    public DbSet<EnergyReading>    EnergyReadings    => Set<EnergyReading>();
    public DbSet<ConsumptionLimit> ConsumptionLimits => Set<ConsumptionLimit>();
    public DbSet<AutomaticAction>  AutomaticActions  => Set<AutomaticAction>();
    public DbSet<Alert>            Alerts            => Set<Alert>();
    public DbSet<DailyReport>      DailyReports      => Set<DailyReport>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        foreach (var et in mb.Model.GetEntityTypes())
        {
            if (typeof(IEntity).IsAssignableFrom(et.ClrType))
                mb.Entity(et.ClrType).Ignore("DomainEvents");
        }

        base.OnModelCreating(mb);
    }

    /* Unit-of-Work */
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => base.SaveChangesAsync(ct);
}