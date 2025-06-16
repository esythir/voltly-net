using Voltly.Domain.Entities;
using Voltly.Infrastructure.Persistence;

namespace Voltly.Infrastructure.Repositories
{
    public class GenericRepository<T> : Repository<T>
        where T : class, IEntity
    {
        public GenericRepository(VoltlyDbContext ctx) : base(ctx) { }
    }
}