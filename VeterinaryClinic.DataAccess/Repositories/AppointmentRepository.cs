using Microsoft.EntityFrameworkCore;
using VeterinaryClinic.DataAccess.Context;
using VeterinaryClinic.Entities;

namespace VeterinaryClinic.DataAccess.Repositories;

public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
{
    private readonly VeterinaryClinicDbContext _context;

    public AppointmentRepository(VeterinaryClinicDbContext context) : base(context)
    {
        _context = context;
    }

    // yeni metod

    public async Task<IReadOnlyList<Appointment>> GetAllWithAnimalAsync()
    {
        return await _context.Appointments
            .Include(a => a.Animal)
            .AsNoTracking()
            .ToListAsync();
    }
}
