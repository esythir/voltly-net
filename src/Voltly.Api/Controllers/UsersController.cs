using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voltly.Application.DTOs;
using Voltly.Application.DTOs.Users;
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

    /// <summary>Criar novo usuário (registro público)</summary>
    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> Register(RegisterUserRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new RegisterUserCommand(request), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Buscar por id (Admin ou próprio usuário)</summary>
    [HttpGet("{id:long}"), Authorize(Roles = "ADMIN,USER")]
    public async Task<UserResponse> GetById(long id, CancellationToken ct)
        => await _mediator.Send(new GetUserByIdQuery(id), ct);

    /// <summary>Listar usuários paginados</summary>
    [HttpGet, Authorize(Roles = "ADMIN")]
    public async Task<PagedResponse<UserResponse>> List([FromQuery] ListUsersQuery query, CancellationToken ct)
        => await _mediator.Send(query, ct);

    /// <summary>Buscar por nome (ADMIN)</summary>
    [HttpGet("search"), Authorize(Roles = "ADMIN")]
    public async Task<IEnumerable<UserResponse>> Search([FromQuery] string name, CancellationToken ct)
        => await _mediator.Send(new SearchUsersQuery(name), ct);

    /// <summary>Atualizar usuário (self ou admin)</summary>
    [HttpPut("{id:long}"), Authorize(Roles = "ADMIN,USER")]
    public async Task<UserResponse> Update(long id, UpdateUserRequest req, CancellationToken ct)
        => await _mediator.Send(new UpdateUserCommand(id, req), ct);
    
}
