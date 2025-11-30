namespace VeterinaryClinic.Messaging;


// Randevu oluştuğunda kuyrukta taşıyacağımız sade event modeli
public class AppointmentCreatedEvent
{
    public int AppointmentId { get; set; }
    public int AnimalId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }

    // İleride mail atmak istersek işimize yarayacak kodlar:
    public string? OwnerFullName { get; set; }
    public string? OwnerEmail { get; set; }
}
