namespace VeterinaryClinic.Entities;

public class WeatherInfo : BaseEntity
{
    public string City { get; set; } = null!;
    public decimal Temperature { get; set; }
    public string Description { get; set; } = null!;

    public DateTime Date { get; set; }
}
