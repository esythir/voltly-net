using Mapster;
using MediatR;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Voltly.Application.Abstractions;
using Voltly.Application.Mapping;
using Voltly.Infrastructure.Persistence;
using Voltly.Infrastructure.Repositories;
using Voltly.Application.Features.Users.Commands.CreateUser;

var builder = WebApplication.CreateBuilder(args);
var cfg     = builder.Configuration;
var connStr = cfg.GetConnectionString("Oracle");

// 1) DbContext com Lazy-Loading só em runtime
builder.Services.AddDbContext<VoltlyDbContext>(opt =>
    opt.UseOracle(connStr,
            o => o.MigrationsAssembly(typeof(VoltlyDbContext).Assembly.FullName))
        .UseLazyLoadingProxies());

// 2) UoW e Repositório genérico
builder.Services.AddScoped<IUnitOfWork, VoltlyDbContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

// 3) Mapster
var mapsterCfg = TypeAdapterConfig.GlobalSettings;
MapsterConfig.Configure(mapsterCfg);
builder.Services.AddSingleton(mapsterCfg);
builder.Services.AddScoped<IMapper, ServiceMapper>();

// 4) MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateUserCommand>());

// 5) FluentValidation
builder.Services
    .AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<CreateUserValidator>();

// 6) Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 7) Auto-migrations ao subir
using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<VoltlyDbContext>();
db.Database.Migrate();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();