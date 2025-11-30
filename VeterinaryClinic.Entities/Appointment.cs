using System.Text.Json.Serialization;

namespace VeterinaryClinic.Entities;

public class Appointment : BaseEntity
{
    public int AnimalId { get; set; }
    [JsonIgnore]
    public Animal? Animal { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }

    public AppointmentStatus Status { get; set; }
    public string? Notes { get; set; }

    // Navigasyon
    [JsonIgnore]
    public ICollection<Treatment> Treatments { get; set; } = new List<Treatment>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

}
