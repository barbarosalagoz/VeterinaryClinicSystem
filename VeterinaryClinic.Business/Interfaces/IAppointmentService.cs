using VeterinaryClinic.Entities;

namespace VeterinaryClinic.Business.Interfaces;

public interface IAppointmentService
{
    // Tüm randevuları asenkron getir
    Task<IReadOnlyList<Appointment>> GetAllAsync();

    // Id'si verilen randevuyu getir (bulamazsa null)
    Task<Appointment?> GetByIdAsync(int id);

    // Yeni randevu oluştur ve geriye oluşturulan randevuyu döndür
    Task<Appointment> CreateAsync(Appointment appointment);
}
