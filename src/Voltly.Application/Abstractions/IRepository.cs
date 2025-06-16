using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Voltly.Application.Abstractions;


public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> Queryable(bool asNoTracking = true);

    ValueTask<TEntity?> GetAsync(long id, CancellationToken ct = default);

    Task AddAsync(TEntity entity, CancellationToken ct = default);

    void Update(TEntity entity);

    Task DeleteAsync(TEntity entity, CancellationToken ct = default);
}