using System.Collections.Generic;

namespace Voltly.Application.Abstractions;

public interface IMapper
{
    TDestination Map<TDestination>(object source);
    TDestination Map<TSource, TDestination>(TSource source);
    IEnumerable<TDestination> MapCollection<TSource, TDestination>(IEnumerable<TSource> sources);
}