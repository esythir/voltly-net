using Microsoft.EntityFrameworkCore;
using Voltly.Application.Abstractions;
using Voltly.Domain.Entities;
using Voltly.Infrastructure.Persistence;

namespace Voltly.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly VoltlyDbContext _db;
    public UserRepository(VoltlyDbContext db) => _db = db;

    public Task<User?> GetByIdAsync(long id, CancellationToken ct = default) =>
        _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        _db.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower(), ct);

    // 🔧  COUNT(*)  →  evita SELECT … THEN True/False
    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default) =>
        await _db.Users.CountAsync(u => u.Email.ToLower() == email.ToLower(), ct) > 0;
    

    public void Update(User u)  => _db.Users.Update(u);
    public void Delete(User user)  => _db.Users.Remove(user);

    public async Task<(IReadOnlyList<User>, long)> GetPagedAsync(
        int page, int size, string? nameLike, CancellationToken ct = default)
    {
        var q = _db.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(nameLike))
            q = q.Where(u => EF.Functions.Like(u.Name.ToLower(), $"%{nameLike.ToLower()}%"));

        var total = await q.LongCountAsync(ct);

        var items = await q
            .OrderBy(u => u.Name)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(ct);

        return (items, total);
    }

    public Task<List<User>> SearchByNameAsync(string term, CancellationToken ct = default) =>
        _db.Users.AsNoTracking()
            .Where(u => EF.Functions.Like(u.Name.ToLower(), $"%{term.ToLower()}%"))
            .OrderBy(u => u.Name)
            .ToListAsync(ct);

    public IQueryable<User> Query() => _db.Users;
    
    public IQueryable<User> Queryable(bool asNoTracking = true)
        => asNoTracking ? _db.Users.AsNoTracking() : _db.Users;

    public ValueTask<User?> GetAsync(long id, CancellationToken ct = default)
        => _db.Users.FindAsync([id], ct);
    
    public Task AddAsync(User user, CancellationToken ct = default)
        => _db.Users.AddAsync(user, ct).AsTask();
    
    public Task AddAsyncRange(IEnumerable<User> users, CancellationToken ct = default)
        => _db.Users.AddRangeAsync(users, ct);

    public Task DeleteAsync(User e, CancellationToken ct = default)
    { _db.Users.Remove(e); return Task.CompletedTask; }
    
    public Task RemoveAsync(User u, CancellationToken ct = default)
    { _db.Users.Remove(u); return Task.CompletedTask; }
    
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}
