using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voltly.Api.Extensions;
using Voltly.Application.DTOs;
using Voltly.Application.DTOs.Users;
using Voltly.Application.Features.Users.Commands.CreateUser;
using Voltly.Application.Features.Users.Commands.DeleteUser;
using Voltly.Application.Features.Users.Commands.RegisterUser;
using Voltly.Application.Features.Users.Commands.UpdateUser;
using Voltly.Application.Features.Users.Queries.GetUserById;
using Voltly.Application.Features.Users.Queries.ListUsers;
using Voltly.Application.Features.Users.Queries.SearchUsers;

namespace Voltly.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator) => _mediator = mediator;
    
    [HttpPost("register"), AllowAnonymous]
    public async Task<IActionResult> Register(RegisterUserRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new RegisterUserCommand(request), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
    
    [HttpPost, Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Create(AdminCreateUserRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateUserCommand(
            request.Name,
            request.Email,
            request.Password,
            request.BirthDate,
            request.Role), ct);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
    
    [HttpGet("{id:long}"), Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult<UserResponse>> GetById(long id, CancellationToken ct)
    {
        if (!User.IsAdmin() && User.GetUserId() != id)
            return Forbid();

        var user = await _mediator.Send(new GetUserByIdQuery(id), ct);
        return Ok(user);
    }
    
    [HttpGet, Authorize(Roles = "ADMIN")]
    public Task<PagedResponse<UserResponse>> List([FromQuery] ListUsersQuery q, CancellationToken ct)
        => _mediator.Send(q, ct);

    [HttpGet("search"), Authorize(Roles = "ADMIN")]
    public Task<IEnumerable<UserResponse>> Search([FromQuery] string name, CancellationToken ct)
        => _mediator.Send(new SearchUsersQuery(name), ct);
    
    [HttpPut("{id:long}"), Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult<UserResponse>> Update(long id, UpdateUserRequest req, CancellationToken ct)
    {
        if (!User.IsAdmin() && User.GetUserId() != id)
            return Forbid();

        var updated = await _mediator.Send(new UpdateUserCommand(id, req), ct);
        return Ok(updated);
    }
    
    [HttpDelete("{id:long}"), Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteUserCommand(id), ct);
        return NoContent();
    }
}
