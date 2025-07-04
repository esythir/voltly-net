using BCrypt.Net;
using Mapster;
using MediatR;
using Voltly.Application.Abstractions;
using Voltly.Application.DTOs.Users;
using Voltly.Domain.Entities;
using Voltly.Domain.Exceptions;
using Voltly.Domain.Enums;

namespace Voltly.Application.Features.Users.Commands.UpdateUser;

public sealed class UpdateUserHandler
    : IRequestHandler<UpdateUserCommand, UserResponse>
{
    private readonly IUserRepository _repo;
    private readonly IUnitOfWork     _uow;
    private readonly IMapper         _map;

    public UpdateUserHandler(IUserRepository repo, IUnitOfWork uow, IMapper map) =>
        (_repo, _uow, _map) = (repo, uow, map);

    public async Task<UserResponse> Handle(UpdateUserCommand cmd, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(cmd.Id, ct)
                   ?? throw new DomainException($"User {cmd.Id} not found.");
        
        cmd.Request.Adapt(user);
        
        if (!string.IsNullOrWhiteSpace(cmd.Request.Password))
            user.Password = BCrypt.Net.BCrypt.HashPassword(cmd.Request.Password);

        user.OnUpdate();

        _repo.Update(user);
        await _uow.CommitAsync(ct);

        return _map.Map<UserResponse>(user);
    }
}