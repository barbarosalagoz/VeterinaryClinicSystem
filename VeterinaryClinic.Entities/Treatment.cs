using System.Text.Json.Serialization;

namespace VeterinaryClinic.Entities;

public class Treatment : BaseEntity
{
    public int AppointmentId { get; set; }

    [JsonIgnore]
    public Appointment? Appointment { get; set; }

    public TreatmentType TreatmentType { get; set; }

    public string? Notes { get; set; }

    public decimal Cost { get; set; }
}
