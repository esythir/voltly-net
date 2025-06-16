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
        AppMapper       mapper)
        => (_uow, _repo, _mapper) = (uow, repo, mapper);

    public async Task<UserResponse> Handle(
        RegisterUserCommand cmd,
        CancellationToken   ct)
    {
        /* 1. e-mail único -------------------------------------- */
        if (await _repo.ExistsByEmailAsync(cmd.Request.Email, ct))
            throw new DomainException($"E-mail '{cmd.Request.Email}' already registered.");

        /* 2. materializa entidade ------------------------------ */
        var entity = cmd.Request.Adapt<User>();
        entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password);
        entity.Role     = UserRole.User;
        entity.OnCreate();                       // carimba datas

        /* 3. salva --------------------------------------------- */
        await _repo.AddAsync(entity, ct);        // ainda sem Id
        await _uow.CommitAsync(ct);

        /* 4. RELÊ para obter Id e colunas geradas -------------- */
        var fresh = await _repo.GetByEmailAsync(cmd.Request.Email, ct)
                    ?? throw new InvalidOperationException(
                        "Erro ao reler o usuário recém-criado.");

        /* 5. devolve DTO --------------------------------------- */
        return _mapper.Map<UserResponse>(fresh);
    }
}