using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Voltly.Application.Abstractions;
using Voltly.Application.Mapping;
using Voltly.Infrastructure.Persistence;
using Voltly.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
var cfg = builder.Configuration;

// DbContext + Oracle + proxies
builder.Services.AddDbContext<VoltlyDbContext>(opt =>
    opt.UseOracle(
            cfg.GetConnectionString("Oracle"),
            o => o.MigrationsAssembly(typeof(VoltlyDbContext).Assembly.FullName))
        .UseLazyLoadingProxies());

// Mapster
var mapsterCfg = TypeAdapterConfig.GlobalSettings;
MapsterConfig.Configure(mapsterCfg);
builder.Services.AddSingleton(mapsterCfg);
builder.Services.AddScoped<IMapper, ServiceMapper>();

// MediatR
builder.Services.AddMediatR(cfgM =>
    cfgM.RegisterServicesFromAssemblyContaining<IUnitOfWork>());

// Repositórios + UoW
builder.Services.Scan(scan => scan
    .FromAssemblies(typeof(Repository<>).Assembly)
    .AddClasses(c => c.AssignableTo(typeof(IRepository<>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());
builder.Services.AddScoped<IUnitOfWork, VoltlyDbContext>();

// Web API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();