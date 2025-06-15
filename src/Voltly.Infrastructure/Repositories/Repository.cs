using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Voltly.Application.Abstractions;
using Voltly.Domain.Entities;
using Voltly.Infrastructure.Persistence;

namespace Voltly.Infrastructure.Repositories;

public abstract class Repository<T> : IRepository<T> where T : class, IEntity
{
    protected readonly VoltlyDbContext _ctx;
    protected readonly DbSet<T> _set;

    protected Repository(VoltlyDbContext ctx)
    {
        _ctx = ctx;
        _set = ctx.Set<T>();
    }

    public Task AddAsync(T entity, CancellationToken ct = default) =>
        _set.AddAsync(entity, ct).AsTask();

    public Task<T?> GetAsync(long id, CancellationToken ct = default) =>
        _set.FirstOrDefaultAsync(e => e.Id == id, ct);

    public IQueryable<T> Query(Expression<Func<T, bool>>? filter = null, bool tracking = false)
    {
        var query = tracking ? _set : _set.AsNoTracking();
        return filter is null ? query : query.Where(filter);
    }

    public Task RemoveAsync(T entity)
    {
        _set.Remove(entity);
        return Task.CompletedTask;
    }
}