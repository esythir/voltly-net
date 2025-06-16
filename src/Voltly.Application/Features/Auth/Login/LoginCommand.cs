using MediatR;
using Voltly.Application.DTOs.Auth;

namespace Voltly.Application.Features.Auth.Login;

public sealed record LoginCommand(LoginRequest Request) : IRequest<AuthResponse>;