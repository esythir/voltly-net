using Ardalis.GuardClauses;
using BCrypt.Net;
using MediatR;
using Voltly.Application.Abstractions;
using Voltly.Application.DTOs;
using Voltly.Domain.Entities;
using Voltly.Domain.Enums;

namespace Voltly.Application.Features.Users.Commands.CreateUser;

public sealed class CreateUserHandler
    : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IRepository<User> _repo;
    private readonly IMapper           _map;
    private readonly IUnitOfWork       _uow;

    public CreateUserHandler(IRepository<User> repo,
        IMapper           map,
        IUnitOfWork       uow) =>
        (_repo, _map, _uow) = (repo, map, uow);

    public async Task<UserDto> Handle(CreateUserCommand cmd, CancellationToken ct)
    {
        Guard.Against.Null(cmd);

        var user = _map.Map<CreateUserCommand, User>(cmd);

        user.Role     = Enum.TryParse<UserRole>(cmd.Role, true, out var roleEnum)
            ? roleEnum
            : UserRole.User;
        user.Password = BCrypt.Net.BCrypt.HashPassword(cmd.Password);
        user.OnCreate();

        await _repo.AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        return _map.Map<User, UserDto>(user);
    }
}