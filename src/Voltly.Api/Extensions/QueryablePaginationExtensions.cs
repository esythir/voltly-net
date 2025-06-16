using Microsoft.EntityFrameworkCore;

namespace Voltly.Api.Extensions;

public static class QueryablePaginationExtensions
{
    public static IQueryable<T> Paginate<T>(
        this IQueryable<T> query, int page, int size)
        where T : class
    {
        page = page <= 0 ? 1 : page;
        size = size <= 0 ? 10 : size;

        return query.AsNoTracking()
            .Skip((page - 1) * size)
            .Take(size);
    }
}