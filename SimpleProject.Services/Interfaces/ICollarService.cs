using SimpleProject.Domain.Entities;

namespace SimpleProject.Services.Interfaces;

public interface ICollarService
{
    Task<Collar> CreateAsync(Collar collar, CancellationToken ct);
    Task<Collar?> GetByIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<Collar>> GetAllAsync(CancellationToken ct);
    Task<Collar> UpdateAsync(int id, Collar collar, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
    Task<bool> CodeExistsAsync(string code, CancellationToken ct);
}