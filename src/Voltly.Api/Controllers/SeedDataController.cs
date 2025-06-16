using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Voltly.Infrastructure.Persistence;

namespace Voltly.Api.Controllers;

/// <summary>
/// Seeds a complete test dataset for the professor / reviewer.
/// </summary>
/// <remarks>
/// • Creates user <b>user@voltly.dev</b> with password <i>Voltly@123</i>.<br/>
/// • Inserts one equipment, one sensor, 24 h of readings, a daily report,
///   monthly limit, alert and an automatic shutdown action.<br/>
/// • If the seed already exists it is deleted first, then recreated.<br/>
/// <br/>
/// <b>How to use</b><br/>
/// 1. Log in with any <i>ADMIN</i> account and press “Authorize” in Swagger.<br/>
/// 2. Call <c>POST /api/seed/demo</c>.<br/>
/// 3. Log in with the generated user and try every endpoint – the data is ready.
/// </remarks>
[ApiController]
[Route("api/seed")]
[Authorize(Roles = "ADMIN")]
public sealed class SeedDataController : ControllerBase
{
    private readonly VoltlyDbContext     _db;
    private readonly IWebHostEnvironment _env;

    public SeedDataController(VoltlyDbContext db, IWebHostEnvironment env)
        => (_db, _env) = (db, env);

    [HttpPost("teacher")]
    public async Task<IActionResult> TeacherSeed(CancellationToken ct = default)
    {
        var sqlPath = Path.Combine(_env.ContentRootPath, "Sql", "teacher-seed.sql");
        if (!System.IO.File.Exists(sqlPath))
            return NotFound($"SQL script not found at '{sqlPath}'");

        var sql = await System.IO.File.ReadAllTextAsync(sqlPath, ct);
        await _db.Database.ExecuteSqlRawAsync(sql, ct);

        return NoContent();
    }
}