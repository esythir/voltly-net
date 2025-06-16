using System;
using System.Threading;
using System.Threading.Tasks;

namespace Voltly.Application.Abstractions;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}