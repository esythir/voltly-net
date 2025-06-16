using Mapster;
using MapsterMapper;
using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Voltly.Application.Abstractions;
using Voltly.Application.Features.Users.Queries.ListUsers;
using Voltly.Application.Mapping;
using Voltly.Infrastructure.Persistence;
using Voltly.Infrastructure.Repositories;

namespace Voltly.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVoltlyInfrastructure(
        this IServiceCollection services, IConfiguration cfg)
    {
        /* DbContext + Oracle -------------------------------------------------- */
        services.AddDbContext<VoltlyDbContext>(opt =>
            opt.UseOracle(
                    cfg.GetConnectionString("Oracle"),
                    o => o.MigrationsAssembly(typeof(VoltlyDbContext).Assembly.FullName))
                .UseLazyLoadingProxies());

        // ➜  MESMA instância para IUnitOfWork
        services.AddScoped<IUnitOfWork>(
            sp => sp.GetRequiredService<VoltlyDbContext>());

        /* Mapster ------------------------------------------------------------- */
        var cfgMap = TypeAdapterConfig.GlobalSettings;
        MapsterConfig.Configure(cfgMap);
        services.AddSingleton(cfgMap);

        services.AddScoped<Voltly.Application.Abstractions.IMapper, ServiceMapper>();
        services.AddScoped<MapsterMapper.IMapper>(
            sp => new Mapper(sp.GetRequiredService<TypeAdapterConfig>()));

        /* Mediatr + FluentValidation ----------------------------------------- */
        services.AddMediatR(opt =>
            opt.RegisterServicesFromAssemblyContaining<ListUsersQuery>());
        services.AddValidatorsFromAssemblyContaining<ListUsersQuery>();

        /* Repositórios -------------------------------------------------------- */
        services.Scan(s => s
            .FromAssembliesOf(typeof(UserRepository))
            .AddClasses(c => c.AssignableTo(typeof(IRepository<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
