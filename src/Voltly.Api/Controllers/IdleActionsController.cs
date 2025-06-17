using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voltly.Application.DTOs;
using Voltly.Application.Features.IdleActions.CheckIdle;

namespace Voltly.Api.Controllers;

[ApiController, Route("api/idle-actions"), Authorize(Roles = "ADMIN,USER")]
public sealed class IdleActionsController : ControllerBase
{
    private readonly IMediator _mediator;
    public IdleActionsController(IMediator mediator) => _mediator = mediator;
    
    /// <summary>Checks sensors and, if idleness is confirmed, registers and returns automatic shutdown actions.</summary>
    [HttpGet]
    public async Task<IActionResult> CheckIdle(
        [FromQuery] CheckIdleQuery query,
        CancellationToken ct)
    {
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }
}