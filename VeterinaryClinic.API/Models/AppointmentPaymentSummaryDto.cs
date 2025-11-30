namespace VeterinaryClinic.API.Models;

public class AppointmentPaymentSummaryDto
{
    public int AppointmentId { get; set; }
    public decimal TotalTreatmentCost { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal RemainingBalance { get; set; }
}
