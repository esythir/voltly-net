using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Voltly.Api.Extensions;

public static class QueryLoadExtensions
{
    public static IQueryable<T> IncludeMany<T>(
        this IQueryable<T> query, params string[] includes) where T : class =>
        includes.Aggregate(query, (cur, inc) => cur.Include(inc));

    public static async Task LoadCollectionAsync<TEntity, TProp>(
        this DbContext ctx,
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProp>>> navigation,
        CancellationToken ct = default)
        where TEntity : class
        where TProp   : class
    {
        var entry = ctx.Entry(entity);
        await entry.Collection(navigation).LoadAsync(ct);
    }
}