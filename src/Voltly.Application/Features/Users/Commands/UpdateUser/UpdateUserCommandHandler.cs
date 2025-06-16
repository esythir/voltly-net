using AppMapper = Voltly.Application.Abstractions.IMapper;
using BCrypt.Net;
using Mapster;
using MediatR;
using Voltly.Application.Abstractions;
using Voltly.Application.DTOs.Users;
using Voltly.Domain.Exceptions;

namespace Voltly.Application.Features.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandHandler
    : IRequestHandler<UpdateUserCommand, UserResponse>
{
    private readonly IUserRepository _repo;
    private readonly IUnitOfWork     _uow;
    private readonly AppMapper       _mapper;

    public UpdateUserCommandHandler(IUserRepository repo, IUnitOfWork uow, AppMapper mapper)
        => (_repo, _uow, _mapper) = (repo, uow, mapper);

    public async Task<UserResponse> Handle(UpdateUserCommand cmd, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(cmd.Id, ct)
                     ?? throw new DomainException($"User {cmd.Id} not found.");

        if (!entity.Email.Equals(cmd.Request.Email, StringComparison.OrdinalIgnoreCase) &&
            await _repo.ExistsByEmailAsync(cmd.Request.Email, ct))
            throw new DomainException($"E-mail '{cmd.Request.Email}' already registered.");

        cmd.Request.Adapt(entity);

        if (cmd.Request.Password is not null)
            entity.Password = BCrypt.Net.BCrypt.HashPassword(cmd.Request.Password);

        _repo.Update(entity);
        await _uow.CommitAsync(ct);

        return _mapper.Map<UserResponse>(entity);
    }
}