using System;

namespace VeterinaryClinic.UI.Models.Payments
{
    // API'nin /api/Payments?appointmentId=... endpoint'inden dönen satırların DTO'su
    public class PaymentListItemDto
    {
        public int Id { get; set; }

        // API tarafında tarih alanının adı Date veya PaidAt olabilir;
        // JSON isimleri bire bir tutmuyorsa yine de çalışır, sadece default kalır.
        public DateTime PaidAt { get; set; }

        public decimal Amount { get; set; }

        // API'de PaymentMethod enum dönüyorsun diye varsayıyorum
        public PaymentMethod Method { get; set; }

        public string? Notes { get; set; }
    }
}
