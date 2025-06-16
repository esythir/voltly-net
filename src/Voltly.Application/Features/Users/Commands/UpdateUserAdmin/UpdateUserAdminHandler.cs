using BCrypt.Net;
using MediatR;
using Voltly.Application.Abstractions;
using Voltly.Application.DTOs.Users;
using Voltly.Domain.Entities;
using Voltly.Domain.Enums;
using Voltly.Domain.Exceptions;
using Mapster;

namespace Voltly.Application.Features.Users.Commands.UpdateUserAdmin;

public sealed class UpdateUserAdminHandler
    : IRequestHandler<UpdateUserAdminCommand, UserResponse>
{
    private readonly IUserRepository _repo;
    private readonly IUnitOfWork     _uow;
    private readonly IMapper         _map;

    public UpdateUserAdminHandler(IUserRepository repo, IUnitOfWork uow, IMapper map) =>
        (_repo, _uow, _map) = (repo, uow, map);

    public async Task<UserResponse> Handle(UpdateUserAdminCommand cmd, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(cmd.Id, ct)
                   ?? throw new DomainException($"User {cmd.Id} not found.");

        cmd.Request.Adapt(user); 
        
        if (!string.IsNullOrWhiteSpace(cmd.Request.Password))
            user.Password = BCrypt.Net.BCrypt.HashPassword(cmd.Request.Password);
        
        user.Role = Enum.Parse<UserRole>(cmd.Request.Role, true);
        user.IsActive = cmd.Request.IsActive;
        user.OnUpdate();

        _repo.Update(user);
        await _uow.CommitAsync(ct);
        return _map.Map<UserResponse>(user);
    }
}