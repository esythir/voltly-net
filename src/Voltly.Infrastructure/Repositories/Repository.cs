using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Voltly.Application.Abstractions;
using Voltly.Domain.Entities;
using Voltly.Infrastructure.Persistence;

namespace Voltly.Infrastructure.Repositories;

public abstract class Repository<T> : IRepository<T> where T : class, IEntity
{
    protected readonly VoltlyDbContext _ctx;
    protected readonly DbSet<T>        _set;

    protected Repository(VoltlyDbContext ctx)
    {
        _ctx = ctx;
        _set = ctx.Set<T>();
    }

    public IQueryable<T> Queryable(bool asNoTracking = true) =>
        asNoTracking ? _set.AsNoTracking() : _set;

    public ValueTask<T?> GetAsync(long id, CancellationToken ct = default) =>
        _set.FindAsync([id], ct);

    public Task AddAsync(T entity, CancellationToken ct = default) =>
        _set.AddAsync(entity, ct).AsTask();

    public Task AddAsyncRange(IEnumerable<T> entities, CancellationToken ct = default) =>
        _set.AddRangeAsync(entities, ct);

    public virtual void Update(T entity) => _set.Update(entity);

    public Task RemoveAsync(T entity, CancellationToken ct = default)
    {
        _set.Remove(entity);
        return Task.CompletedTask;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        _ctx.SaveChangesAsync(ct);
}