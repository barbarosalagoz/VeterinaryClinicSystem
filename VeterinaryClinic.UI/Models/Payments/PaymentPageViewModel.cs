namespace VeterinaryClinic.UI.Models.Payments;

public class PaymentPageViewModel
{
    public int AppointmentId { get; set; }

    public string? AnimalName { get; set; }
    public string? Species { get; set; }
    public string? Date { get; set; }
    public string? Time { get; set; }

    public decimal TotalTreatmentCost { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal RemainingBalance { get; set; }

    public List<PaymentListItemViewModel> Payments { get; set; } = new();

    public PaymentCreateViewModel NewPayment { get; set; } = new();


}
