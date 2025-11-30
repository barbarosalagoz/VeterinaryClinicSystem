namespace VeterinaryClinic.UI.Models.Payments;

public class AppointmentPaymentSummaryViewModel
{
    public int AppointmentId { get; set; }
    public decimal TotalTreatmentCost { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal RemainingBalance { get; set; }
}
