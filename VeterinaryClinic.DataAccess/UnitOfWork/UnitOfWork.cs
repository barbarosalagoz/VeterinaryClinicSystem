using VeterinaryClinic.DataAccess.Context;
using VeterinaryClinic.DataAccess.Repositories;
using VeterinaryClinic.Entities;

namespace VeterinaryClinic.DataAccess.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly VeterinaryClinicDbContext _context;

    public IGenericRepository<User> Users { get; }
    public IGenericRepository<Animal> Animals { get; }
    public IAppointmentRepository Appointments { get; }
    public IGenericRepository<Treatment> Treatments { get; }
    public IGenericRepository<Payment> Payments { get; }
    public IGenericRepository<WeatherInfo> WeatherInfos { get; }

    public UnitOfWork(VeterinaryClinicDbContext context)
    {
        _context = context;

        Users = new GenericRepository<User>(_context);
        Animals = new GenericRepository<Animal>(_context);
        Appointments = new AppointmentRepository(_context);
        Treatments = new GenericRepository<Treatment>(_context);
        Payments = new GenericRepository<Payment>(_context);
        WeatherInfos = new GenericRepository<WeatherInfo>(_context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}
