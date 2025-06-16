using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voltly.Application.Features.Limits.Commands.RecalculateMonthly;

namespace Voltly.Api.Controllers;

[ApiController, Route("api/limits"), Authorize(Roles = "ADMIN")]
public sealed class LimitsController : ControllerBase
{
    private readonly IMediator _med;
    public LimitsController(IMediator med) => _med = med;
    
    /// <summary>Recalculates consumption limits for the current month.</summary>
    [HttpPost("monthly-recalculation")]
    public Task<IActionResult> RecalculateCurrent(CancellationToken ct)
        => _med.Send(new RecalculateMonthlyCommand(null), ct)
            .ContinueWith(_ => (IActionResult)NoContent(), ct);
    
    /// <summaru>Recalculates consumption limits for a specific year/month (yyyyMM).</summary>
    [HttpPost("monthly-recalculation/{yearMonth:int:min(200001):max(210012)}")]
    public Task<IActionResult> RecalculateForPeriod(int yearMonth, CancellationToken ct)
        => _med.Send(new RecalculateMonthlyCommand(yearMonth), ct)
            .ContinueWith(_ => (IActionResult)NoContent(), ct);
}