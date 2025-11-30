using System;

namespace VeterinaryClinic.Messaging.Events
{
    public class PaymentReceivedEvent
    {
        public int PaymentId { get; set; }
        public int AppointmentId { get; set; }

        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public DateTime PaidAt { get; set; }

        // İstersen ileride doldururuz:
        public int AnimalId { get; set; }
        public string AnimalName { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
    }
}
