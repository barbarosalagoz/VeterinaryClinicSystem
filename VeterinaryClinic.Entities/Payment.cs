using System.Text.Json.Serialization;

namespace VeterinaryClinic.Entities;

public class Payment : BaseEntity
{
    public int AppointmentId { get; set; }

    [JsonIgnore]
    public Appointment? Appointment { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTime PaymentDate { get; set; }

    // Online ödeme bilgileri (isteğe bağlı)
    public string? Provider { get; set; }
    public string? ProviderReference { get; set; }
}
