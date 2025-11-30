using VeterinaryClinic.Messaging;
using VeterinaryClinic.Business.Interfaces;
using VeterinaryClinic.DataAccess.UnitOfWork;
using VeterinaryClinic.Entities;

namespace VeterinaryClinic.Business.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessagePublisher _messagePublisher;

    // Constructor: DI buraya IUnitOfWork entegre edecek

    public AppointmentService(IUnitOfWork unitOfWork, IMessagePublisher messagePublisher)
    {
        _unitOfWork = unitOfWork;
        _messagePublisher = messagePublisher;
    }

    public async Task<IReadOnlyList<Appointment>> GetAllAsync()
    {
        // Tüm randevuları UnitOfWork içindeki Appointment repository'den çekiyoruz
        return await _unitOfWork.Appointments.GetAllAsync();
    }

    public async Task<Appointment?> GetByIdAsync(int id)
    {
        // Belirli bir ID'ye sahip randevuyu çekiyoruz
        return await _unitOfWork.Appointments.GetByIdAsync(id);
    }

    public async Task<Appointment> CreateAsync(Appointment appointment)
    {
        // Eğer Status hiç set edilmemişse, varsayılan olarak Scheduled yapıyoruz
        if (appointment.Status == 0)
        {
            appointment.Status = AppointmentStatus.Scheduled;
        }

        // Yeni randevuyu ekliyoruz (ileride burada tarih saat dolu mu, çakışma var mı vs. eklenecek)
        await _unitOfWork.Appointments.AddAsync(appointment);
        await _unitOfWork.SaveChangesAsync();

        // Event objesini oluştur
        var evt = new AppointmentCreatedEvent
        {
            AppointmentId = appointment.Id,
            AnimalId = appointment.AnimalId,
            Date = appointment.Date,
            Time = appointment.Time,
            // Şimdilik Owner bilgilerini doldurmuycaz ileride include + map
            OwnerFullName = null, //appointment.Animal?.Owner?.FullName,
            OwnerEmail = null //appointment.Animal?.Owner?.Email
        };

        // Kuyruğa göndermek üzere publish çağrısı
        await _messagePublisher.PublishAsync("appointmens.created",evt);

        return appointment;
    }
}
