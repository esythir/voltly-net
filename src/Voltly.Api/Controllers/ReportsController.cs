using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voltly.Application.DTOs;
using Voltly.Application.Features.Reports.Commands.GenerateDaily;
using Voltly.Application.Features.Reports.Queries.GetDailyHistory;
using Voltly.Application.Features.Reports.Queries.GetDailyCo2;

namespace Voltly.Api.Controllers;

[ApiController, Route("api/reports"), Authorize]
public sealed class ReportsController : ControllerBase
{
    private readonly IMediator _med;
    public ReportsController(IMediator med) => _med = med;
    
    /// <summary>Generates a daily consumption report for a device.</summary>
    [HttpPost("daily-consumption"), Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GenerateDaily(
        GenerateDailyReportCommand cmd,
        CancellationToken ct)
    {
        var result = await _med.Send(cmd, ct);
        return Ok(result);
    }
    
    /// <summary>History of daily reports (paginated).</summary>
    [HttpGet("daily-consumption"), Authorize(Roles = "ADMIN,USER")]
    public async Task<IActionResult> History(
        [FromQuery] GetDailyHistoryQuery q,
        CancellationToken ct)
    {
        var result = await _med.Send(q, ct);
        return Ok(result);
    }
    
    /// <summary>Report of CO₂ emissions derived from daily consumption.</summary>
    [HttpGet("daily-consumption/co2"), Authorize(Roles = "ADMIN,USER")]
    public async Task<IActionResult> Co2(
        [FromQuery] GetDailyCo2Query q,
        CancellationToken ct)
    {
        var result = await _med.Send(q, ct);
        return Ok(result);
    }
}