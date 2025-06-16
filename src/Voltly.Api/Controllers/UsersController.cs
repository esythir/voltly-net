using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voltly.Api.Extensions;
using Voltly.Application.DTOs;
using Voltly.Application.DTOs.Users;
using Voltly.Application.Features.Users.Commands.CreateUser;
using Voltly.Application.Features.Users.Commands.DeactivateUser;
using Voltly.Application.Features.Users.Commands.UpdateUser;
using Voltly.Application.Features.Users.Commands.UpdateUserAdmin;
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
    
    /// <summary>Creates a new user account (ADMIN only).</summary>
    [HttpPost, Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Create(AdminCreateUserRequest req, CancellationToken ct)
    {
        var dto = await _mediator.Send(new CreateUserCommand(
            req.Name, req.Email, req.Password, req.BirthDate, req.Role), ct);
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }
    
    /// <summary>Updates only the user's own profile (fields Name, Email, Password, and BirthDate).</summary>
    [HttpPut("profile"), Authorize(Roles = "ADMIN,USER")]
    public async Task<UserResponse> UpdateOwn(UpdateProfileRequest req, CancellationToken ct)
    {
        var id = User.GetUserId();
        return await _mediator.Send(new UpdateUserCommand(id, req), ct);
    }
    
    /// <summary>Updates a user's profile (ADMIN only).</summary>
    [HttpPut("{id:long}"), Authorize(Roles = "ADMIN")]
    public async Task<UserResponse> Update(long id, UpdateUserAdminRequest req, CancellationToken ct) =>
        await _mediator.Send(new UpdateUserAdminCommand(id, req), ct);
    
    /// <summary>Deactivates the user's own account (sets IsActive to false).</summary>
    [HttpDelete("profile"), Authorize(Roles = "ADMIN,USER")]
    public async Task<IActionResult> DeactivateOwn(CancellationToken ct)
    {
        await _mediator.Send(new DeactivateUserCommand(User.GetUserId()), ct);
        return NoContent();
    }
    
    /// <summary>Deactivates a user account (sets IsActive to false) (ADMIN only).</summary>
    [HttpDelete("{id:long}"), Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        await _mediator.Send(new Voltly.Application.Features.Users.Commands.DeleteUser.DeleteUserCommand(id), ct);
        return NoContent();
    }
    
    /// <summary>Gets the user's own profile.</summary>
    [HttpGet("{id:long}"), Authorize(Roles = "ADMIN,USER")]
    public async Task<ActionResult<UserResponse>> GetById(long id, CancellationToken ct)
    {
        if (!User.IsAdmin() && User.GetUserId() != id)
            return Forbid();

        var dto = await _mediator.Send(new GetUserByIdQuery(id), ct);
        return Ok(dto);
    }
    
    /// <summary>Lists all users (ADMIN only).</summary>
    [HttpGet, Authorize(Roles = "ADMIN")]
    public Task<PagedResponse<UserResponse>> List([FromQuery] ListUsersQuery q, CancellationToken ct) =>
        _mediator.Send(q, ct);

    /// <summary>Searches users by name (ADMIN only).</summary>
    [HttpGet("search"), Authorize(Roles = "ADMIN")]
    public Task<IEnumerable<UserResponse>> Search(string name, CancellationToken ct) =>
        _mediator.Send(new SearchUsersQuery(name), ct);
}
