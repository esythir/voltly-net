using AppMapper = Voltly.Application.Abstractions.IMapper;
using MediatR;
using Voltly.Application.Abstractions;
using Voltly.Application.DTOs;
using Voltly.Application.DTOs.Users;
using Voltly.Domain.Entities;

namespace Voltly.Application.Features.Users.Queries.ListUsers;

public sealed class ListUsersQueryHandler
    : IRequestHandler<ListUsersQuery, PagedResponse<UserResponse>>
{
    private readonly IUserRepository _repo;
    private readonly AppMapper       _mapper;
    public ListUsersQueryHandler(IUserRepository repo, AppMapper mapper)
        => (_repo, _mapper) = (repo, mapper);

    public async Task<PagedResponse<UserResponse>> Handle(ListUsersQuery q, CancellationToken ct)
    {
        (var list, var total) = await _repo.GetPagedAsync(q.Page, q.Size, q.NameLike, ct);
        var dto = _mapper.MapCollection<User, UserResponse>(list).ToList();
        return new(dto, total, q.Page, q.Size);
    }
}