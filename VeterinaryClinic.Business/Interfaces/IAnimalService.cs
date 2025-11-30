using VeterinaryClinic.Entities;

namespace VeterinaryClinic.Business.Interfaces;

public interface IAnimalService
{
    Task<IReadOnlyList<Animal>> GetAllAsync();
    Task<Animal?> GetByIdAsync(int id);
    Task<Animal> CreateAsync(Animal animal);
}
