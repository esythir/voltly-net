using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voltly.Application.DTOs.Auth;
using Voltly.Application.Features.Auth.Login;

namespace Voltly.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator) => _mediator = mediator;
    
    [HttpPost("login"), AllowAnonymous]
    public async Task<AuthResponse> Login(LoginRequest request, CancellationToken ct)
        => await _mediator.Send(new LoginCommand(request), ct);
}