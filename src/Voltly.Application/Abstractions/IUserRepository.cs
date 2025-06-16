using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Voltly.Domain.Entities;

namespace Voltly.Application.Abstractions;

public interface IUserRepository : IRepository<User>
{
    Task<User?>   GetByIdAsync(long id,              CancellationToken ct = default);
    Task<User?>   GetByEmailAsync(string email,      CancellationToken ct = default);
    Task<bool>    ExistsByEmailAsync(string email,   CancellationToken ct = default);

    Task<(IReadOnlyList<User>, long)> GetPagedAsync(
        int page, int size, string? nameLike,        CancellationToken ct = default);

    Task<List<User>> SearchByNameAsync(
        string namePart,                             CancellationToken ct = default);
}