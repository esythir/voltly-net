using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voltly.Application.DTOs.Auth;
using Voltly.Application.DTOs.Users;
using Voltly.Application.Features.Auth.Login;
using Voltly.Application.Features.Users.Commands.RegisterUser;

namespace Voltly.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator) => _mediator = mediator;
    
    /// <summary>Login: authenticates a user and returns a JWT token.</summary>
    [HttpPost("login"), AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new LoginCommand(request), ct);
        return Ok(result);
    }
    
    /// <summary>Sign-up: creates a USER account and returns a token.</summary>
    [HttpPost("register"), AllowAnonymous]
    public async Task<IActionResult> Register(RegisterUserRequest request, CancellationToken ct)
    {
        var reg = await _mediator.Send(new RegisterUserCommand(request), ct);
        var loginReq = new LoginRequest(reg.Email, request.Password);
        var result = await _mediator.Send(new LoginCommand(loginReq), ct);
        return Ok(result);
    }
}