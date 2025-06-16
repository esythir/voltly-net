using MediatR;
using Voltly.Application.DTOs.Users;

namespace Voltly.Application.Features.Users.Queries.SearchUsers;

public sealed record SearchUsersQuery(string NamePart) : IRequest<IEnumerable<UserResponse>>;