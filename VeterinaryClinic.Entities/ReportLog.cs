using System;

namespace VeterinaryClinic.Entities
{
    public class ReportLog : BaseEntity
    {
        // Örneğin: "PaymentReceived", "AppointmentCreated"
        public string EventType { get; set; } = null!;

        // JSON olarak ham mesaj
        public string Payload { get; set; } = null!;

        // Olayın gerçekleştiği zaman
        public DateTime OccurredAt { get; set; }
    }
}
