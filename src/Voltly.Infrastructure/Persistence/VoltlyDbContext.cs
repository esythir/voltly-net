using Microsoft.EntityFrameworkCore;
using Voltly.Domain.Entities;
using Voltly.Domain;

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
        // Fluent-API overrides & indexes:
        mb.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        mb.Entity<Sensor>()
            .HasIndex(s => s.SerialNumber)
            .IsUnique();

        // Hooks
        foreach (var et in mb.Model.GetEntityTypes())
        {
            if (typeof(IEntity).IsAssignableFrom(et.ClrType))
            {
                mb.Entity(et.ClrType).Ignore("DomainEvents"); // DDD events
            }
        }
        base.OnModelCreating(mb);
    }

    /* Unit-of-Work */
    public Task<int> CommitAsync(CancellationToken ct = default) => base.SaveChangesAsync(ct);
}