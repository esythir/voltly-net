using Microsoft.EntityFrameworkCore;
using Voltly.Api.Extensions;           // extensão que registra toda a infraestrutura
using Voltly.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// registra DbContext, UoW, Mapster, MediatR, FluentValidation, repositórios, etc.
builder.Services.AddVoltlyInfrastructure(builder.Configuration);

// MVC + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// aplica migrações na primeira subida
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<VoltlyDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();