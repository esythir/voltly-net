using Microsoft.EntityFrameworkCore;
using Voltly.Application.Abstractions;

namespace Voltly.Infrastructure.Repositories;

public static class RepositoryExtensions
{
    public static async Task AddAsyncRange<T>(
        this IRepository<T> repo,
        IEnumerable<T> items,
        CancellationToken ct = default) where T : class
    {
        foreach (var item in items)
            await repo.AddAsync(item, ct);
    }
    
    public static async Task<PagedResult<T>> ToPagedAsync<T>(
        this IQueryable<T> source,
        int page,
        int size,
        CancellationToken ct = default)
    {
        page = page < 1 ? 1 : page;
        size = size < 1 ? 20 : size;

        var total = await source.CountAsync(ct);
        var data  = await source
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(ct);

        return new PagedResult<T>(data, total);
    }
}

public sealed record PagedResult<T>(IReadOnlyList<T> Data, int Total);