using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Voltly.Application.Abstractions; 
using Voltly.Domain.Entities;
using Voltly.Infrastructure.Persistence;

namespace Voltly.Infrastructure.Repositories;

public sealed class EquipmentRepository
    : Repository<Equipment>, IEquipmentRepository
{
    public EquipmentRepository(VoltlyDbContext ctx) : base(ctx) { }

    public Task<Equipment?> GetByNameAsync(string name, CancellationToken ct = default) =>
        _set.AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name.ToLower() == name.ToLower(), ct);

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default) =>
        await _set.CountAsync(e => e.Name.ToLower() == name.ToLower(), ct) > 0;

    public IQueryable<Equipment> IncludeOwnerAndSensors(bool tracking = false) =>
        (tracking ? _set : _set.AsNoTracking())
        .Include(e => e.Owner)
        .Include(e => e.Sensors);
}