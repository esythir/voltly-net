using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Voltly.Application.Abstractions;
using Voltly.Application.Mapping;
using Voltly.Infrastructure.Persistence;
using Voltly.Infrastructure.Repositories;

namespace Voltly.Api.Extensions;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddVoltlyInfrastructure(
        this IServiceCollection services,
        IConfiguration cfg)
    {
        /* DbContext + Oracle */
        services.AddDbContext<VoltlyDbContext>(opt =>
            opt.UseOracle(
                    cfg.GetConnectionString("Oracle"),
                    o => o.MigrationsAssembly(typeof(VoltlyDbContext).Assembly.FullName))
                .UseLazyLoadingProxies());

        services.AddScoped<IUnitOfWork, VoltlyDbContext>();

        /* Mapster */
        var mapCfg = TypeAdapterConfig.GlobalSettings;
        MapsterConfig.Configure(mapCfg);
        services.AddSingleton(mapCfg);
        services.AddScoped<IMapper, ServiceMapper>();

        /* Repositories */
        services.Scan(scan => scan
            .FromAssemblies(typeof(Repository<>).Assembly)
            .AddClasses(c => c.AssignableTo(typeof(IRepository<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}