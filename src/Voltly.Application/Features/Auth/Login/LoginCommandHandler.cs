using AppMapper = Voltly.Application.Abstractions.IMapper;
using BCrypt.Net;
using MediatR;
using Voltly.Application.Abstractions;
using Voltly.Application.DTOs.Auth;
using Voltly.Application.DTOs.Users;
using Voltly.Domain.Exceptions;

namespace Voltly.Application.Features.Auth.Login;

public sealed class LoginCommandHandler
    : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IUserRepository    _repo;
    private readonly IJwtTokenGenerator _jwt;
    private readonly AppMapper          _map;

    public LoginCommandHandler(
        IUserRepository    repo,
        IJwtTokenGenerator jwt,
        AppMapper          map)
        => (_repo, _jwt, _map) = (repo, jwt, map);

    public async Task<AuthResponse> Handle(LoginCommand cmd, CancellationToken ct)
    {
        var req  = cmd.Request;
        var user = await _repo.GetByEmailAsync(req.Email, ct)
                   ?? throw new DomainException("Invalid credentials.");

        if (!user.IsActive || !BCrypt.Net.BCrypt.Verify(req.Password, user.Password))
            throw new DomainException("Invalid credentials.");

        var token = _jwt.GenerateToken(user.Id, user.Email, user.Role.ToString(), out var expires);

        var dtoUser = _map.Map<UserResponse>(user);
        return new AuthResponse(token, expires, dtoUser);
    }
}