using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voltly.Application.DTOs;
using Voltly.Application.Features.Alerts.Commands.GenerateAlerts;
using Voltly.Application.Features.Alerts.Queries.ListAlerts;

namespace Voltly.Api.Controllers;

[ApiController, Route("api/alerts"), Authorize]
public sealed class AlertsController : ControllerBase
{
    private readonly IMediator _med;
    public AlertsController(IMediator med) => _med = med;
    
    /// <summary>Generates consumption alerts for all devices.</summary>
    [HttpPost, Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Generate(CancellationToken ct)
    {
        var result = await _med.Send(new GenerateAlertsCommand(), ct);
        return Ok(result);
    }
    
    /// <summary>Searches for alerts (optional filters).</summary>
    [HttpGet, Authorize(Roles = "ADMIN,USER")]
    public async Task<IActionResult> List(
        [FromQuery] ListAlertsQuery q,
        CancellationToken ct)
    {
        var result = await _med.Send(q, ct);
        return Ok(result);
    }
}