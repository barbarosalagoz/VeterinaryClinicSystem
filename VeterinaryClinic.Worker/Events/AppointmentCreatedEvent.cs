namespace VeterinaryClinic.Worker.Events;

public class AppointmentCreatedEvent
{
    public int AppointmentId { get; set; }
    public int AnimalId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public string? OwnerFullName { get; set; }
    public string? OwnerEmail { get; set; }
}
