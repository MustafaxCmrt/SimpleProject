using SimpleProject.Domain.Entities;

namespace SimpleProject.Services.Interfaces;

public interface IAnimalService
{
    Task<Animal> CreateAsync(Animal animal, CancellationToken ct);
    Task<Animal?> GetByIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<Animal>> GetAllAsync(CancellationToken ct);
    Task<Animal> UpdateAsync(int id, Animal animal, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}