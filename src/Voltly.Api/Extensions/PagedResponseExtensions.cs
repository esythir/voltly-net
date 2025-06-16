using Mapster;

namespace Voltly.Api.Extensions;

public static class PagedResponseExtensions
{
    public static PagedResponse<TOut> Map<TIn,TOut>(
        this PagedResponse<TIn> src,
        Func<TIn,TOut> selector)
        => new(src.Data.Select(selector).ToList(),
            src.Total, src.Page, src.Size);
}