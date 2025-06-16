using Microsoft.EntityFrameworkCore;
using Voltly.Domain.Entities;
using Voltly.Application.Abstractions;

namespace Voltly.Infrastructure.Persistence;

public sealed class VoltlyDbContext : DbContext, IUnitOfWork
{
    public VoltlyDbContext(DbContextOptions<VoltlyDbContext> opt) 
        : base(opt) { }

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
        // converte todos os bools para NUMBER(1)
        foreach (var et in mb.Model.GetEntityTypes())
        {
            foreach (var pi in et.ClrType.GetProperties().Where(p => p.PropertyType == typeof(bool)))
            {
                mb.Entity(et.ClrType)
                    .Property(pi.Name)
                    .HasConversion<int>()
                    .HasColumnType("NUMBER(1)");
            }
        }
        mb.ApplyConfigurationsFromAssembly(typeof(VoltlyDbContext).Assembly);
    }

    // override em vez de esconder o método base
    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
        => base.SaveChangesAsync(ct);
}