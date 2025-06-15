using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Voltly.Infrastructure.Persistence;
using Voltly.Domain;
using Voltly.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var cfg = builder.Configuration;

// DbContext
builder.Services.AddDbContext<VoltlyDbContext>(opt =>
{
    opt.UseOracle(cfg.GetConnectionString("Oracle"))
        .UseLazyLoadingProxies();
});

// Mapster
var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
Voltly.Application.Mapping.MapsterConfig.Configure(typeAdapterConfig);
builder.Services.AddSingleton(typeAdapterConfig);
builder.Services.AddScoped<IMapper, ServiceMapper>();

// DI genéricos
builder.Services.Scan(scan => scan
    .FromAssemblies(
        typeof(Voltly.Infrastructure.Repositories.Repository<>).Assembly,
        typeof(Voltly.Application.Mapping.MapsterConfig).Assembly)
    .AddClasses(c => c.AssignableTo(typeof(IRepository<>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.Services.AddScoped<IUnitOfWork, VoltlyDbContext>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();