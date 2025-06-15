using System.Linq.Expressions;

namespace Voltly.Domain;

public interface IRepository<T> where T : class, IEntity
{
    Task<T?>      GetAsync(long id, CancellationToken ct = default);
    IQueryable<T> Query(Expression<Func<T,bool>>? filter = null, bool tracking = false);
    Task AddAsync     (T entity, CancellationToken ct = default);
    Task RemoveAsync  (T entity);
}