using System;
using System.Collections.Generic;
using Mapster;
using Voltly.Application.Abstractions;

namespace Voltly.Application.Mapping;

public sealed class ServiceMapper : IMapper, MapsterMapper.IMapper
{
    private readonly TypeAdapterConfig   _cfg;
    private readonly MapsterMapper.Mapper _mapper;

    public ServiceMapper(TypeAdapterConfig cfg)
    {
        _cfg    = cfg;
        _mapper = new MapsterMapper.Mapper(_cfg);
    }
    
    public TDestination Map<TDestination>(object src) =>
        _mapper.Map<TDestination>(src);

    public TDestination Map<TSource, TDestination>(TSource src) =>
        _mapper.Map<TSource, TDestination>(src);

    public IEnumerable<TDestination> MapCollection<TSource, TDestination>(
        IEnumerable<TSource> src) =>
        _mapper.Map<IEnumerable<TDestination>>(src);
    
    TypeAdapterConfig MapsterMapper.IMapper.Config => _cfg;

    Mapster.ITypeAdapterBuilder<TSource>
        MapsterMapper.IMapper.From<TSource>(TSource src) => _mapper.From(src);

    object MapsterMapper.IMapper.Map(object source, Type sourceType, Type destinationType) =>
        _mapper.Map(source, sourceType, destinationType);

    object MapsterMapper.IMapper.Map(
        object source, object destination, Type sourceType, Type destinationType) =>
        _mapper.Map(source, destination, sourceType, destinationType);

    TDestination MapsterMapper.IMapper.Map<TSource, TDestination>(
        TSource source, TDestination destination) =>
        _mapper.Map(source, destination);
}