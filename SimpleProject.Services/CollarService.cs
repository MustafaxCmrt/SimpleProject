using Microsoft.EntityFrameworkCore;
using SimpleProject.Data;
using SimpleProject.Domain.Entities;
using SimpleProject.Services.Interfaces;

namespace SimpleProject.Services;

public class CollarService : ICollarService
{
    private readonly AppDbContext _db;
    public CollarService(AppDbContext db) { _db = db; }

    public async Task<bool> CodeExistsAsync(string code, CancellationToken ct)
    {
        return await _db.Collars.AnyAsync(x => x.Code == code && !x.Deleted, ct);
    }

    public async Task<Collar> CreateAsync(Collar collar, CancellationToken ct)
    {
        var exists = await CodeExistsAsync(collar.Code, ct);
        if (exists) throw new InvalidOperationException("Code already exists");
        _db.Collars.Add(collar);
        await _db.SaveChangesAsync(ct);
        return collar;
    }

    public async Task<Collar?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _db.Collars
            .Include(c => c.Animal)
            .Include(c => c.OwnerUser)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<IReadOnlyList<Collar>> GetAllAsync(CancellationToken ct)
    {
        return await _db.Collars
            .Include(c => c.Animal)
            .Include(c => c.OwnerUser)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<Collar> UpdateAsync(int id, Collar collar, CancellationToken ct)
    {
        if (id != collar.Id) throw new ArgumentException("Id mismatch");
        _db.Entry(collar).State = EntityState.Modified;
        await _db.SaveChangesAsync(ct);
        return collar;
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var e = await _db.Collars.FindAsync(new object?[] { id }, ct);
        if (e == null) return;
        _db.Collars.Remove(e);
        await _db.SaveChangesAsync(ct);
    }
}