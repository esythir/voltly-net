using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Voltly.Api.Extensions;
using Voltly.Api.Swagger;
using Voltly.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Infrastructure (DbContext, Repos, Mapster, etc.)
builder.Services.AddVoltlyInfrastructure(builder.Configuration);

// JWT Authentication & Authorization
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

// Controllers
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "Voltly IoT API",
        Version     = "v1",
        Description = "IoT platform for energy management with real-time consumption monitoring and remote device shutdown via RESTful microservice."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);

    // JWT Bearer security definition
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.Http,
        Scheme       = "bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header,
        Description = "Enter **Bearer &lt;token&gt;**. " +
                      "Click the _Authorize_ button and paste the JWT."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        [ new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id   = "Bearer"
            }
        } ] = Array.Empty<string>()
    });
    
    c.TagActionsBy(api => new[] { api.ActionDescriptor.RouteValues["controller"]! });
    
    c.DocumentFilter<TagDescriptionsDocumentFilter>();
});

var app = builder.Build();

// Automatic migrations (development only)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<VoltlyDbContext>();
    db.Database.Migrate();
}

// Middleware pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Voltly IoT API v1");
    c.DocumentTitle = "Voltly IoT API Documentation";
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
