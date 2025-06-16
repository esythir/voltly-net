using AppMapper = Voltly.Application.Abstractions.IMapper;
using MediatR;
using Voltly.Application.Abstractions;
using Voltly.Application.DTOs.Users;
using Voltly.Domain.Exceptions;

namespace Voltly.Application.Features.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler
    : IRequestHandler<GetUserByIdQuery, UserResponse>
{
    private readonly IUserRepository _repo;
    private readonly AppMapper       _mapper;

    public GetUserByIdQueryHandler(IUserRepository repo, AppMapper mapper)
        => (_repo, _mapper) = (repo, mapper);

    public async Task<UserResponse> Handle(GetUserByIdQuery q, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(q.Id, ct)
                   ?? throw new DomainException($"User {q.Id} not found.");
        return _mapper.Map<UserResponse>(user);
    }
}