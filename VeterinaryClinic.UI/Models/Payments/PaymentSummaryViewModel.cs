using System;
using System.Collections.Generic;

namespace VeterinaryClinic.UI.Models.Payments
{
    // Sadece özet ViewModel burada kalsın.
    // PaymentCreateViewModel ve PaymentListItemViewModel
    // zaten ayrı dosyalarda tanımlı.

    public class PaymentSummaryViewModel
    {
        // Randevu bilgileri
        public int AppointmentId { get; set; }
        public string AnimalName { get; set; } = "";
        public string Species { get; set; } = "";
        public string Date { get; set; } = "";   // "2025-11-27"
        public string Time { get; set; } = "";   // "14:30"

        // Bakiye özeti
        public decimal TotalTreatmentCost { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal RemainingBalance => TotalTreatmentCost - TotalPaid;

        // Var olan ödemeler tablosu
        public IReadOnlyList<PaymentListItemViewModel> Payments { get; set; }
            = Array.Empty<PaymentListItemViewModel>();

        // Alt taraftaki "Add payment" formu
        public PaymentCreateViewModel NewPayment { get; set; }
            = new PaymentCreateViewModel();
    }
}
