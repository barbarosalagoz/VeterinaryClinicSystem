using VeterinaryClinic.Business.Interfaces;
using VeterinaryClinic.DataAccess.UnitOfWork;
using VeterinaryClinic.Entities;

namespace VeterinaryClinic.Business.Services;

public class TreatmentService : ITreatmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public TreatmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IReadOnlyList<Treatment>> GetByAppointmentIdAsync(int appointmentId)
    {
        return await _unitOfWork.Treatments.FindAsync(t => t.AppointmentId == appointmentId);
    }
    public async Task<Treatment?> GetByIdAsync(int id)
    {
        return await _unitOfWork.Treatments.GetByIdAsync(id);
    }
    public async Task<Treatment> CreateAsync(Treatment treatment)
    {
        await _unitOfWork.Treatments.AddAsync(treatment);
        await _unitOfWork.SaveChangesAsync();
        return treatment;
    }
}
