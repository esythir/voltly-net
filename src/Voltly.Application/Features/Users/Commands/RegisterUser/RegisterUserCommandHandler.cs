using AppMapper = Voltly.Application.Abstractions.IMapper;
using BCrypt.Net;
using Mapster;
using MediatR;
using Voltly.Application.Abstractions;
using Voltly.Application.DTOs.Users;
using Voltly.Domain.Entities;
using Voltly.Domain.Enums;
using Voltly.Domain.Exceptions;

namespace Voltly.Application.Features.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler
    : IRequestHandler<RegisterUserCommand, UserResponse>
{
    private readonly IUnitOfWork     _uow;
    private readonly IUserRepository _repo;
    private readonly AppMapper       _mapper;

    public RegisterUserCommandHandler(
        IUnitOfWork     uow,
        IUserRepository repo,
        AppMapper       mapper) =>
        (_uow, _repo, _mapper) = (uow, repo, mapper);

    public async Task<UserResponse> Handle(
        RegisterUserCommand cmd,
        CancellationToken   ct)
    {
        /* 1. e-mail único (case-insensitive) */
        if (await _repo.ExistsByEmailAsync(cmd.Request.Email, ct))
            throw new DomainException(
                $"E-mail '{cmd.Request.Email}' already registered.");

        /* 2. cria entidade */
        var entity      = cmd.Request.Adapt<User>();
        entity.Email    = entity.Email.ToLowerInvariant();
        entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password);
        entity.Role     = UserRole.User;
        entity.OnCreate();

        /* 3. persiste e COMMIT (o Id será preenchido) */
        await _repo.AddAsync(entity, ct);
        await _uow.CommitAsync(ct);

        /* 4. devolve DTO – não é necessário reler no banco */
        return _mapper.Map<UserResponse>(entity);
    }
}