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
    public Task<int> Generate(CancellationToken ct) =>
        _med.Send(new GenerateAlertsCommand(), ct);
    
    /// <summary>Searches for alerts (optional filters).</summary>
    [HttpGet, Authorize(Roles = "ADMIN,USER")]
    public Task<PagedResponse<AlertDto>> List(
        [FromQuery] ListAlertsQuery q,
        CancellationToken ct) => _med.Send(q, ct);
}