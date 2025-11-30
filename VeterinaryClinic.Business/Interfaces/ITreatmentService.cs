using VeterinaryClinic.Entities;

namespace VeterinaryClinic.Business.Interfaces;

public interface ITreatmentService
{
    // Belirli bir randevunun tüm tedavilerini getir
    Task<IReadOnlyList<Treatment>> GetByAppointmentIdAsync(int appointmentId);

    // Tek bir tedavi
    Task<Treatment?> GetByIdAsync(int id);

    // Yeni tedavi ekle
    Task<Treatment> CreateAsync(Treatment treatment);
}
