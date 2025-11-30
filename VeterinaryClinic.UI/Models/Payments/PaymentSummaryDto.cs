namespace VeterinaryClinic.UI.Models.Payments
{
    // /api/Payments/summary?appointmentId=... endpoint'inin DTO'su
    public class PaymentSummaryDto
    {
        public int AppointmentId { get; set; }
        public decimal TotalTreatmentCost { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal RemainingBalance { get; set; }
    }
}
