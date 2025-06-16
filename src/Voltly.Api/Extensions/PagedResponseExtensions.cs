using System.Linq;
using Voltly.Application.DTOs;

namespace Voltly.Api.Extensions;

public static class PagedResponseExtensions
{
    public static PagedResponse<TOut> Map<TIn, TOut>(
        this PagedResponse<TIn> src,
        Func<TIn, TOut> selector) =>
        new(src.Items.Select(selector).ToList(),
            src.Total, src.Page, src.Size);
}