using System;

namespace VeterinaryClinic.UI.Models.Payments
{
    public class PaymentListItemViewModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }

        // Ödeme tarihi
        public DateTime PaidAt { get; set; }

        // Enum'u string olarak göstermek için
        public string? MethodText { get; set; }

        public string? Notes { get; set; }

    }
}
