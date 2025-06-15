namespace Voltly.Application.Abstractions
{
    public interface IMapper
    {
        TDestination Map<TSource, TDestination>(TSource source);
        IEnumerable<TDestination> MapCollection<TSource, TDestination>(IEnumerable<TSource> sources);
    }
}