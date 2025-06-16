using Microsoft.EntityFrameworkCore;

namespace Voltly.Api.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResponse<T>> ToPagedAsync<T>(
        this IQueryable<T> query,
        int page,
        int size,
        CancellationToken ct = default)
    {
        page = page < 1 ? 1 : page;
        size = size < 1 ? 10 : size;

        var total = await query.CountAsync(ct);
        var data  = await query.Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(ct);

        return new PagedResponse<T>(data, total, page, size);
    }
}