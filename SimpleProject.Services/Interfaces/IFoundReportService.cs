using SimpleProject.Domain.Entities;

namespace SimpleProject.Services.Interfaces;

public interface IFoundReportService
{
    Task<FoundReport> CreateAsync(FoundReport item, CancellationToken ct);
    Task<FoundReport?> GetByIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<FoundReport>> GetAllAsync(CancellationToken ct);
    Task<FoundReport> UpdateAsync(int id, FoundReport item, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}