using AppMapper = Voltly.Application.Abstractions.IMapper;
using MediatR;
using Voltly.Application.Abstractions;
using Voltly.Application.DTOs.Users;
using Voltly.Domain.Entities;

namespace Voltly.Application.Features.Users.Queries.SearchUsers;

public sealed class SearchUsersQueryHandler
    : IRequestHandler<SearchUsersQuery, IEnumerable<UserResponse>>
{
    private readonly IUserRepository _repo;
    private readonly AppMapper       _mapper;
    public SearchUsersQueryHandler(IUserRepository repo, AppMapper mapper)
        => (_repo, _mapper) = (repo, mapper);

    public async Task<IEnumerable<UserResponse>> Handle(SearchUsersQuery q, CancellationToken ct)
    {
        var list = await _repo.SearchByNameAsync(q.NamePart, ct);
        return _mapper.MapCollection<User, UserResponse>(list);
    }
}