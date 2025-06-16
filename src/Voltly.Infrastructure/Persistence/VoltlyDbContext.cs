using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion; 
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
    
    protected override void ConfigureConventions(ModelConfigurationBuilder cfg)
    {
        cfg.Properties<bool>()
            .HaveConversion<BoolToZeroOneConverter<int>>()
            .HaveColumnType("NUMBER(1)");
    }
    
    protected override void OnModelCreating(ModelBuilder mb) =>
        mb.ApplyConfigurationsFromAssembly(typeof(VoltlyDbContext).Assembly);

    public override Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        base.SaveChangesAsync(ct);

    public Task CommitAsync(CancellationToken ct = default) =>
        SaveChangesAsync(ct);
}