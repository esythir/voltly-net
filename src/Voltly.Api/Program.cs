using Microsoft.EntityFrameworkCore;
using Voltly.Api.Extensions;
using Voltly.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddVoltlyInfrastructure(builder.Configuration);

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