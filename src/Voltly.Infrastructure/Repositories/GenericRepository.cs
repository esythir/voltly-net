using Microsoft.EntityFrameworkCore;
using Voltly.Application.Abstractions;
using Voltly.Infrastructure.Persistence;

namespace Voltly.Infrastructure.Repositories;

public class GenericRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly VoltlyDbContext _db;
    private readonly DbSet<TEntity>  _set;

    public GenericRepository(VoltlyDbContext db)
    { _db = db; _set = db.Set<TEntity>(); }

    public IQueryable<TEntity> Queryable(bool noTracking = true)
        => noTracking ? _set.AsNoTracking() : _set;

    public IQueryable<TEntity> Query() => _set;

    public ValueTask<TEntity?> GetAsync(long id, CancellationToken ct = default)
        => _set.FindAsync([id], ct);

    public Task AddAsync(TEntity entity, CancellationToken ct = default)
        => _set.AddAsync(entity, ct).AsTask();

    public Task AddAsyncRange(IEnumerable<TEntity> entities, CancellationToken ct = default)
        => _set.AddRangeAsync(entities, ct);

    public void Update(TEntity entity) => _set.Update(entity);

    public Task RemoveAsync(TEntity entity, CancellationToken ct = default)
    { _set.Remove(entity); return Task.CompletedTask; }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}