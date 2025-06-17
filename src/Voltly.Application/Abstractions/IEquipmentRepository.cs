using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Voltly.Domain.Entities;

namespace Voltly.Application.Abstractions;

public interface IEquipmentRepository : IRepository<Equipment>
{
    Task<Equipment?> GetByNameAsync(string name, CancellationToken ct = default);
    Task<bool>       ExistsByNameAsync(string name, CancellationToken ct = default);
    
    IQueryable<Equipment> IncludeOwnerAndSensors(bool tracking = false);
}