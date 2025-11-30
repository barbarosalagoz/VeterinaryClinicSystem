using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VeterinaryClinic.UI.Models.Payments
{
    // Yeni ödeme eklerken kullanılacak model
    public class PaymentCreateViewModel
    {
        public int AppointmentId { get; set; }

        public decimal Amount { get; set; }

        // Ödeme metodu (enum/int)
        public PaymentMethod Method { get; set; }

        public string? Notes { get; set; }

        // Dropdown için seçenekler
        public IEnumerable<SelectListItem> PaymentMethods { get; set; }
            = Array.Empty<SelectListItem>();
    }
}
