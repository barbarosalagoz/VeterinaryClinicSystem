namespace VeterinaryClinic.UI.Models.Appointments;

public class AppointmentListItemViewModel
{
    public int Id { get; set; }
    public string AnimalName { get; set; } = "";
    public string Species { get; set; } = "";
    public string Date { get; set; } = "";
    public string Time { get; set; } = "";
    public string StatusText { get; set; } = "";
}
