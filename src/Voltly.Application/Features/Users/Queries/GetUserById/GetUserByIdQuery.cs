using MediatR;
using Voltly.Application.DTOs.Users;

namespace Voltly.Application.Features.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(long Id) : IRequest<UserResponse>;