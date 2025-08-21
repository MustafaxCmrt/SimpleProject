using Microsoft.EntityFrameworkCore;
using SimpleProject.Data;
using SimpleProject.Domain.Entities;
using SimpleProject.Services.Interfaces;

namespace SimpleProject.Services;

public class FoundReportService : IFoundReportService
{
    private readonly AppDbContext _db;
    public FoundReportService(AppDbContext db) { _db = db; }

    public async Task<FoundReport> CreateAsync(FoundReport item, CancellationToken ct)
    {
        _db.FoundReports.Add(item);
        await _db.SaveChangesAsync(ct);
        return item;
    }

    public async Task<FoundReport?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _db.FoundReports
            .Include(x => x.Collar)
            .Include(x => x.Animal)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<IReadOnlyList<FoundReport>> GetAllAsync(CancellationToken ct)
    {
        return await _db.FoundReports
            .Include(x => x.Collar)
            .Include(x => x.Animal)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<FoundReport> UpdateAsync(int id, FoundReport item, CancellationToken ct)
    {
        if (id != item.Id) throw new ArgumentException("Id mismatch");
        _db.Entry(item).State = EntityState.Modified;
        await _db.SaveChangesAsync(ct);
        return item;
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var e = await _db.FoundReports.FindAsync(new object?[] { id }, ct);
        if (e == null) return;
        _db.FoundReports.Remove(e);
        await _db.SaveChangesAsync(ct);
    }
}