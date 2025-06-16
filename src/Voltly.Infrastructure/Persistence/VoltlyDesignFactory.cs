using Oracle.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Voltly.Infrastructure.Persistence;

public sealed class VoltlyDesignFactory : IDesignTimeDbContextFactory<VoltlyDbContext>
{
    public VoltlyDbContext CreateDbContext(string[] args)
    {
        var root = Path.GetFullPath(Path.Combine(
            Directory.GetCurrentDirectory(), "..", ".."));

        var cfg = new ConfigurationBuilder()
            .SetBasePath(root)
            .AddJsonFile("src/Voltly.Api/appsettings.json",           optional: true)
            .AddJsonFile("src/Voltly.Api/appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connection =
            Environment.GetEnvironmentVariable("VOLTLY_CONN")
            ?? cfg.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection not found.  • Defina VOLTLY_CONN   OU   " +
                "• Coloque \"DefaultConnection\" em appsettings.");

        var options = new DbContextOptionsBuilder<VoltlyDbContext>()
            .UseOracle(connection, o =>
            {
                o.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion21);
                o.MigrationsAssembly(typeof(VoltlyDbContext).Assembly.FullName);
            })
            .Options;

        return new VoltlyDbContext(options);
    }
}