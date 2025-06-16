using Microsoft.EntityFrameworkCore;
using Voltly.Application.Abstractions;
using Voltly.Infrastructure.Persistence;

namespace Voltly.Infrastructure.Repositories
{
    public class GenericRepository<T> : IRepository<T>
        where T : class
    {
        protected readonly VoltlyDbContext _ctx;
        protected readonly DbSet<T>        _set;

        public GenericRepository(VoltlyDbContext ctx)
        {
            _ctx = ctx;
            _set = ctx.Set<T>();
        }

        public IQueryable<T> Queryable(bool asNoTracking = true)
            => asNoTracking ? _set.AsNoTracking() : _set;

        public ValueTask<T?> GetAsync(long id, CancellationToken ct = default)
            => _set.FindAsync(new object[] { id }, ct);

        public Task AddAsync(T entity, CancellationToken ct = default)
            => _set.AddAsync(entity, ct).AsTask();

        public Task DeleteAsync(T entity, CancellationToken ct = default)
        {
            _set.Remove(entity);
            return Task.CompletedTask;
        }

        public void Update(T entity) => _set.Update(entity);
    }
}