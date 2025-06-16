using Oracle.EntityFrameworkCore;             
using Microsoft.EntityFrameworkCore;
using Voltly.Infrastructure.Persistence;
using Voltly.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddVoltlyInfrastructure(builder.Configuration);

var connStr = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<VoltlyDbContext>(opt =>
    opt.UseOracle(
            connStr,
            oracle =>
            {
                // PARA Oracle 19 c use 21 c-compat             👇
                oracle.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion21);
                oracle.MigrationsAssembly(typeof(VoltlyDbContext).Assembly.FullName);
            })
        .UseLazyLoadingProxies());

// MVC + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<VoltlyDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();