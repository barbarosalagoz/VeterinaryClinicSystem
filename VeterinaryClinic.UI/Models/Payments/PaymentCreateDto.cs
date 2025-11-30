namespace VeterinaryClinic.UI.Models.Payments
{
    // UI'dan API'ye POST ettiğimiz istek DTO'su
    public class PaymentCreateDto
    {
        public int AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public string? Notes { get; set; }
    }
}
