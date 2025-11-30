namespace VeterinaryClinic.UI.Models;

public class AnimalViewModel
{
    public int Id { get; set; }
    public int OwnerId { get; set; }

    public string? Name { get; set; }
    public int Age { get; set; }

    public decimal Weight { get; set; }
    public decimal Height { get; set; }

    public string? Species { get; set; }
    public string? Breed { get; set; } 
    
    public string? MedicalHistory { get; set; }
}
