using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Voltly.Infrastructure.Persistence;

public class VoltlyDesignFactory : IDesignTimeDbContextFactory<VoltlyDbContext>
{
    public VoltlyDbContext CreateDbContext(string[] args)
    {
        var cfg = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .AddEnvironmentVariables()
            .Build();

        var opt = new DbContextOptionsBuilder<VoltlyDbContext>()
            .UseOracle(cfg.GetConnectionString("Oracle"),
                o => o.MigrationsAssembly(typeof(VoltlyDbContext).Assembly.FullName))
            .UseLazyLoadingProxies()
            .EnableSensitiveDataLogging()
            .Options;

        return new VoltlyDbContext(opt);
    }
}