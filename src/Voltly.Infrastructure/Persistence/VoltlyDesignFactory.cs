// VoltlyDesignFactory.cs
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Voltly.Infrastructure.Persistence;

public sealed class VoltlyDesignFactory : IDesignTimeDbContextFactory<VoltlyDbContext>
{
    public VoltlyDbContext CreateDbContext(string[] args)
    {
        var cfg = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var opts = new DbContextOptionsBuilder<VoltlyDbContext>()
            .UseOracle(
                cfg.GetConnectionString("Oracle"),
                o => o.MigrationsAssembly(typeof(VoltlyDbContext).Assembly.FullName)
            );

        return new VoltlyDbContext(opts.Options);
    }
}