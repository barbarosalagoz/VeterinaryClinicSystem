namespace VeterinaryClinic.UI.Models;

public class WeatherViewModel
{
    public string? City { get; set; }
    public double Temperature { get; set; }
    public string? Description { get; set; }
    public string IconCode { get; set; } = "01d";
}
