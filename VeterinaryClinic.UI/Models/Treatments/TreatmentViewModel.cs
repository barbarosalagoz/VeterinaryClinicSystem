namespace VeterinaryClinic.UI.Models.Treatments;

public class TreatmentViewModel
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public string TreatmentType { get; set; } // Örn: "Muayene", "Aşı", "Röntgen"
    public string Notes { get; set; }
    public decimal Cost { get; set; }
}