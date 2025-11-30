using VeterinaryClinic.Business.Interfaces;
using VeterinaryClinic.DataAccess.UnitOfWork;
using VeterinaryClinic.Entities;

namespace VeterinaryClinic.Business.Services;

public class AnimalService : IAnimalService
{
    private readonly IUnitOfWork _unitOfWork;

    public AnimalService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<Animal>> GetAllAsync()
    {
        return await _unitOfWork.Animals.GetAllAsync();
    }

    public async Task<Animal?> GetByIdAsync(int id)
    {
        return await _unitOfWork.Animals.GetByIdAsync(id);
    }

    public async Task<Animal> CreateAsync(Animal animal)
    {
        await _unitOfWork.Animals.AddAsync(animal);
        await _unitOfWork.SaveChangesAsync();
        return animal;
    }
}
