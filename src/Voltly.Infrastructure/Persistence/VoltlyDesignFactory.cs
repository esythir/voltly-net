using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Voltly.Infrastructure.Persistence;

public sealed class VoltlyDesignFactory : IDesignTimeDbContextFactory<VoltlyDbContext>
{
    public VoltlyDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot cfg = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        var options = new DbContextOptionsBuilder<VoltlyDbContext>()
            .UseOracle(
                cfg.GetConnectionString("Oracle"),
                o => o.MigrationsAssembly(typeof(VoltlyDbContext).Assembly.FullName))
            .UseLazyLoadingProxies()
            .Options;

        return new VoltlyDbContext(options);
    }
}