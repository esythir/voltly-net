using System.Collections.Generic;
using Mapster;
using Voltly.Application.Abstractions;

namespace Voltly.Application.Mapping;

public class ServiceMapper : IMapper
{
    private readonly TypeAdapterConfig _config;
    public ServiceMapper(TypeAdapterConfig config) 
        => _config = config;

    public TDestination Map<TSource, TDestination>(TSource source)
        => source.Adapt<TDestination>(_config);

    public IEnumerable<TDestination> MapCollection<TSource, TDestination>(IEnumerable<TSource> sources)
        => sources.Adapt<IEnumerable<TDestination>>(_config);
}