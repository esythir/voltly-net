using Microsoft.EntityFrameworkCore;
using Voltly.Domain;
using Voltly.Domain.Entities;
using Voltly.Infrastructure.Persistence;

namespace Voltly.Infrastructure.Repositories;

public abstract class Repository<T> : IRepository<T> where T : class, IEntity
{
    protected readonly VoltlyDbContext _ctx;
    protected readonly DbSet<T> _db;

    protected Repository(VoltlyDbContext ctx)
    {
        _ctx = ctx;
        _db = ctx.Set<T>();
    }

    public async Task AddAsync(T entity, CancellationToken ct = default) => await _db.AddAsync(entity, ct);

    public async Task<T?> GetAsync(long id, CancellationToken ct = default) =>
        await _db.FirstOrDefaultAsync(e => e.Id == id, ct);

    public IQueryable<T> Query(Expression<Func<T, bool>>? filter = null, bool tracking = false)
    {
        var query = tracking ? _db : _db.AsNoTracking();
        return filter is null ? query : query.Where(filter);
    }

    public Task RemoveAsync(T entity)
    {
        _db.Remove(entity);
        return Task.CompletedTask;
    }
}