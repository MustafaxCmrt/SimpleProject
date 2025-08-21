using Microsoft.EntityFrameworkCore;
using SimpleProject.Data;
using SimpleProject.Domain.Entities;
using SimpleProject.Services.Interfaces;

namespace SimpleProject.Services;

public class AnimalService : IAnimalService
{
    private readonly AppDbContext _db;
    public AnimalService(AppDbContext db) { _db = db; }

    public async Task<Animal> CreateAsync(Animal animal, CancellationToken ct)
    {
        _db.Set<Animal>().Add(animal);
        await _db.SaveChangesAsync(ct);
        return animal;
    }

    public async Task<Animal?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _db.Set<Animal>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<IReadOnlyList<Animal>> GetAllAsync(CancellationToken ct)
    {
        return await _db.Set<Animal>().AsNoTracking().ToListAsync(ct);
    }

    public async Task<Animal> UpdateAsync(int id, Animal animal, CancellationToken ct)
    {
        if (id != animal.Id) throw new ArgumentException("Id mismatch");
        _db.Entry(animal).State = EntityState.Modified;
        await _db.SaveChangesAsync(ct);
        return animal;
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var e = await _db.Set<Animal>().FindAsync(new object?[] { id }, ct);
        if (e == null) return;
        _db.Set<Animal>().Remove(e);
        await _db.SaveChangesAsync(ct);
    }
}