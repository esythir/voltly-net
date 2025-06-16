using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Voltly.Infrastructure.Persistence;

namespace Voltly.Api.Controllers;

/// <summary>
/// Roda o script fixo <c>DataSeed/teacher-seed.sql</c>.
/// POST /api/seed/teacher
/// </summary>
[ApiController]
[Route("api/seed/teacher")] 
[AllowAnonymous] 
public sealed class SeedDataController : ControllerBase
{
    private readonly VoltlyDbContext _db;
    public SeedDataController(VoltlyDbContext db) => _db = db;

    [HttpPost]
    public async Task<IActionResult> Run(CancellationToken ct = default)
    {
        var file = Path.Combine(AppContext.BaseDirectory, "DataSeed", "teacher-seed.sql");
        if (!System.IO.File.Exists(file))
            return NotFound($"SQL script not found at '{file}'");

        var raw = await System.IO.File.ReadAllTextAsync(file, ct);
        if (string.IsNullOrWhiteSpace(raw))
            return BadRequest("SQL script is empty.");
        
        raw = Regex.Replace(raw, @"\r\n?", "\n");
        raw = Regex.Replace(raw, @"--.*?$", string.Empty, RegexOptions.Multiline);

        var batches = new List<string>();
        var buf     = new StringBuilder();
        var inPlSql = false;

        foreach (var line in raw.Split('\n'))
        {
            var t = line.Trim();

            if (!inPlSql && t.StartsWith("BEGIN", StringComparison.OrdinalIgnoreCase))
                inPlSql = true;

            if (inPlSql && t.Equals("END;", StringComparison.OrdinalIgnoreCase))
            {
                buf.AppendLine(line);
                batches.Add(buf.ToString());
                buf.Clear();
                inPlSql = false;
                continue;
            }

            if (!inPlSql && (t == "/" || t.EndsWith(';')))
            {
                buf.AppendLine(t == "/" ? string.Empty : line.Replace(";", ""));
                if (buf.Length > 0) batches.Add(buf.ToString());
                buf.Clear();
            }
            else buf.AppendLine(line);
        }
        if (buf.Length > 0) batches.Add(buf.ToString());
        
        await using var trx = await _db.Database.BeginTransactionAsync(ct);
        foreach (var sql in batches.Select(b => b.Trim()).Where(b => b.Length > 0))
            await _db.Database.ExecuteSqlRawAsync(sql, ct);
        await trx.CommitAsync(ct);

        return NoContent();
    }
}
