namespace Voltly.Infrastructure.Repositories;

public sealed class EquipmentRepository : Repository<Equipment>, IEquipmentRepository
{
    public EquipmentRepository(VoltlyDbContext ctx) : base(ctx) { }

    public Task<Equipment?> GetByNameAsync(string name, CancellationToken ct = default) =>
        _set.AsNoTracking().FirstOrDefaultAsync(e => e.Name.ToLower() == name.ToLower(), ct);

    public Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default) =>
        _set.AnyAsync(e => e.Name.ToLower() == name.ToLower(), ct);

    public IQueryable<Equipment> IncludeOwnerAndSensors() =>
        _set.Include(e => e.Owner)
            .Include(e => e.Sensors);
}
