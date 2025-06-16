using MediatR;
using Voltly.Application.DTOs;
using Voltly.Application.DTOs.Users;

namespace Voltly.Application.Features.Users.Queries.ListUsers;

public sealed record ListUsersQuery(int Page = 1, int Size = 20, string? NameLike = null)
    : IRequest<PagedResponse<UserResponse>>;