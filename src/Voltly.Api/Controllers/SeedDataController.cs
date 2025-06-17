using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Voltly.Infrastructure.Persistence;

namespace Voltly.Api.Controllers;

[ApiController]
[Route("api/seed")]
public sealed class SeedDataController : ControllerBase
{
    private readonly VoltlyDbContext _db;
    public SeedDataController(VoltlyDbContext db) => _db = db;
    
    [HttpPost]
    public async Task<IActionResult> Run(CancellationToken ct = default)
    {
        const string ScriptName = "teacher-seed.sql";
        var file = Path.Combine(AppContext.BaseDirectory, "DataSeed", ScriptName);

        if (!System.IO.File.Exists(file))
            return NotFound($"SQL script not found at '{file}'");

        var raw = await System.IO.File.ReadAllTextAsync(file, ct);
        if (string.IsNullOrWhiteSpace(raw))
            return BadRequest("SQL script is empty.");
        
        raw = Regex.Replace(raw, @"\r\n?", "\n");
        raw = Regex.Replace(raw, @"--.*?$", string.Empty, RegexOptions.Multiline);

        var batches = new List<string>();
        var buffer  = new StringBuilder();
        var inPlSql = false;

        foreach (var line in raw.Split('\n'))
        {
            var trimmed = line.Trim();
            
            if (!inPlSql &&
                (trimmed.StartsWith("DECLARE", StringComparison.OrdinalIgnoreCase) ||
                 trimmed.StartsWith("BEGIN",   StringComparison.OrdinalIgnoreCase)))
                inPlSql = true;
            
            if (inPlSql && trimmed.Equals("END;", StringComparison.OrdinalIgnoreCase))
            {
                buffer.AppendLine(line);
                batches.Add(buffer.ToString());
                buffer.Clear();
                inPlSql = false;
                continue;
            }
            
            if (!inPlSql &&
                (trimmed == "/" || trimmed.EndsWith(";")))
            {
                buffer.AppendLine(trimmed == "/" ? string.Empty : line);
                if (!string.IsNullOrWhiteSpace(buffer.ToString()))
                    batches.Add(buffer.ToString());
                buffer.Clear();
            }
            else
            {
                buffer.AppendLine(line);
            }
        }
        if (buffer.Length > 0) batches.Add(buffer.ToString());
        
        await using var trx = await _db.Database.BeginTransactionAsync(ct);

        foreach (var sql in batches.Where(b => !string.IsNullOrWhiteSpace(b)))
            await _db.Database.ExecuteSqlRawAsync(sql, ct);

        await trx.CommitAsync(ct);
        return NoContent();
    }
}
