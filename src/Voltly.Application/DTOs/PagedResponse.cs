namespace Voltly.Application.DTOs;

public sealed record PagedResponse<T>(
    IReadOnlyList<T> Items,
    long Total,
    int Page,
    int Size);