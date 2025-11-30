namespace VeterinaryClinic.UI.Models;

public class AnimalListItemViewModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }


    public decimal Weight { get; set; }

    public string? Species { get; set; }
    public string? Breed { get; set; }

    public string OwnerName { get; set; }

}
