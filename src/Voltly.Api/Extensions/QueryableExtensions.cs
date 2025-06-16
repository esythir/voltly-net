using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; 

namespace Voltly.Api.Extensions;

/// <summary>
/// Extensões para paginação assíncrona em consultas <see cref="IQueryable{T}"/>.
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Executa a consulta paginada e devolve <see cref="PagedResult{T}"/>.
    /// </summary>
    public static async Task<PagedResult<T>> ToPagedAsync<T>(
        this IQueryable<T> query,
        int page,
        int size,
        CancellationToken ct = default)
    {
        var total = await query.CountAsync(ct);
        var data  = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(ct);

        return new(page, size, total, data);
    }
}

/// <param name="Page">Página solicitada (1-based).</param>
/// <param name="Size">Tamanho da página.</param>
/// <param name="Total">Total de registros na consulta.</param>
/// <param name="Data">Itens da página.</param>
public sealed record PagedResult<T>(
    int Page,
    int Size,
    int Total,
    IReadOnlyCollection<T> Data);