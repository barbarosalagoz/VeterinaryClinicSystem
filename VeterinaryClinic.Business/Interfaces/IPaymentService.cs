using VeterinaryClinic.Entities;

namespace VeterinaryClinic.Business.Interfaces;

public interface IPaymentService
{
    // Belirli bir randevunun tüm ödemelerini getir
    Task<IReadOnlyList<Payment>> GetByAppointmentIdAsync(int appointmentId);
    // Tek bir ödeme
    Task<Payment?> GetByIdAsync(int id);
    // Yeni ödeme ekle
    Task<Payment> CreateAsync(Payment payment);
}
