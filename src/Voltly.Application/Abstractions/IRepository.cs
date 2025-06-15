using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Voltly.Application.Abstractions;

public interface IRepository<T> where T : class
{
    IQueryable<T>   Queryable   (bool asNoTracking = true);
    ValueTask<T?>   GetAsync    (long id,  CancellationToken ct = default);
    Task            AddAsync    (T entity, CancellationToken ct = default);
    Task            DeleteAsync (T entity, CancellationToken ct = default);
}