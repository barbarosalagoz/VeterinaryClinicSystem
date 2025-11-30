using VeterinaryClinic.DataAccess.Repositories;
using VeterinaryClinic.Entities;

namespace VeterinaryClinic.DataAccess.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    IGenericRepository<User> Users { get; }
    IGenericRepository<Animal> Animals { get; }

    IAppointmentRepository Appointments { get; }
   
    IGenericRepository<Treatment> Treatments { get; }
    IGenericRepository<Payment> Payments { get; }
    IGenericRepository<WeatherInfo> WeatherInfos { get; }

    Task<int> SaveChangesAsync();
}
