using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Voltly.Api.Extensions;
using Voltly.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

/* Infra (DbContext, Repos, Mapster, etc.) */
builder.Services.AddVoltlyInfrastructure(builder.Configuration);

/* JWT -------------------------------------------------------------------- */
var jwtCfg = builder.Configuration.GetSection("Jwt");
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer   = jwtCfg["Issuer"],
            ValidAudience = jwtCfg["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtCfg["SecretKey"]!))
        };
    });
builder.Services.AddAuthorization();

/* MVC + Swagger ---------------------------------------------------------- */
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

/* migrations automáticas (DEV/APRENDIZADO) */
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<VoltlyDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();   //  <<--
app.UseAuthorization();    //  <<--

app.MapControllers();
app.Run();