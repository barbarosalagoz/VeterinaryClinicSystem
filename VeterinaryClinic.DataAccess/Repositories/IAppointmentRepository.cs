using VeterinaryClinic.Entities;

namespace VeterinaryClinic.DataAccess.Repositories;

public interface IAppointmentRepository : IGenericRepository<Appointment>
{
    Task<IReadOnlyList<Appointment>> GetAllWithAnimalAsync();
}
