namespace Voltly.Application.Abstractions;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}