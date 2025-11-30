using VeterinaryClinic.Business.Interfaces;
using VeterinaryClinic.DataAccess.UnitOfWork;
using VeterinaryClinic.Entities;
using VeterinaryClinic.Messaging;
using VeterinaryClinic.Messaging.Events;

namespace VeterinaryClinic.Business.Services;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessagePublisher _publisher;

    public PaymentService(
        IUnitOfWork unitOfWork,
        IMessagePublisher publisher)
    {
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task<IReadOnlyList<Payment>> GetByAppointmentIdAsync(int appointmentId)
    {
        return await _unitOfWork.Payments.FindAsync(p => p.AppointmentId == appointmentId);
    }

    public async Task<Payment?> GetByIdAsync(int id)
    {
        return await _unitOfWork.Payments.GetByIdAsync(id);
    }

    public async Task<Payment> CreateAsync(Payment payment)
    {
        // 1) Ödemeyi kaydet
        await _unitOfWork.Payments.AddAsync(payment);
        await _unitOfWork.SaveChangesAsync();

        // 2) Event oluştur
        var evt = new PaymentReceivedEvent
        {
            PaymentId = payment.Id,
            AppointmentId = payment.AppointmentId,
            Amount = payment.AmountPaid,
            Method = payment.PaymentMethod.ToString(),
            PaidAt = payment.PaymentDate,

            AnimalId = 0,
            AnimalName = string.Empty,
            Species = string.Empty
        };

        // 3) RabbitMQ'ya publish
        await _publisher.PublishAsync("vetclinic.payments", evt);

        return payment;
    }
}
