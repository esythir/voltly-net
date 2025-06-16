namespace Voltly.Application.Abstractions;

public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> Queryable(bool asNoTracking = true);
    IQueryable<TEntity> Query() => Queryable();

    ValueTask<TEntity?> GetAsync   (long id, CancellationToken ct = default);
    Task<TEntity?>      GetByIdAsync(long id, CancellationToken ct = default) => GetAsync(id, ct).AsTask();

    Task AddAsync       (TEntity entity,                 CancellationToken ct = default);
    Task AddAsyncRange  (IEnumerable<TEntity> entities,  CancellationToken ct = default);

    void Update         (TEntity entity);
    Task RemoveAsync    (TEntity entity,                 CancellationToken ct = default);

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}