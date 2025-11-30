using VeterinaryClinic.UI.Models;

namespace VeterinaryClinic.UI.Services;

public interface IAnimalApiClient
{
    Task<IReadOnlyList<AnimalListItemViewModel>> GetAnimalsAsync();
}
