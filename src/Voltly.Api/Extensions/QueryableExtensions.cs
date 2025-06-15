using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Voltly.Api.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedAsync<T>(this IQueryable<T> query, int page, int size,
        CancellationToken ct = default)
    {
        var total = await query.CountAsync(ct);
        var data  = await query.Skip((page - 1) * size).Take(size).ToListAsync(ct);
        return new(page, size, total, data);
    }
}

public record PagedResult<T>(int Page, int Size, int Total, IReadOnlyCollection<T> Data);