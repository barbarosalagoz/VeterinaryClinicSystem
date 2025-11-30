using System.Text.Json.Serialization;

namespace VeterinaryClinic.Entities;

public class Animal : BaseEntity
{
    public int OwnerId { get; set; }

    [JsonIgnore]
    public User? Owner { get; set; } = null!;

    public string Name { get; set; } = null!;
    public int Age { get; set; }

    public decimal Weight { get; set; }
    public decimal Height { get; set; }

    public string Species { get; set; } = null!;
    public string? Breed { get; set; }

    public string? MedicalHistory { get; set; }

    // Navigasyon
    [JsonIgnore]
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
