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
    public Task<DailyReportDto> GenerateDaily(
        GenerateDailyReportCommand cmd,
        CancellationToken ct) => _med.Send(cmd, ct);
    
    /// <summary>History of daily reports (paginated).</summary>
    [HttpGet("daily-consumption"), Authorize(Roles = "ADMIN,USER")]
    public Task<PagedResponse<DailyReportDto>> History(
        [FromQuery] GetDailyHistoryQuery q,
        CancellationToken ct) => _med.Send(q, ct);
    
    /// <summary>Report of CO₂ emissions derived from daily consumption.</summary>
    [HttpGet("daily-consumption/co2"), Authorize(Roles = "ADMIN,USER")]
    public Task<IEnumerable<DailyReportDto>> Co2(
        [FromQuery] GetDailyCo2Query q,
        CancellationToken ct) => _med.Send(q, ct);
}