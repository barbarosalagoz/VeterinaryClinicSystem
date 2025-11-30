namespace VeterinaryClinic.Entities;

public class AppointmentLog : BaseEntity
{
    public int AppointmentId { get; set; }
    public int AnimalId { get; set; } 

    public DateTime OccuredAt { get; set; }

    public string? OwnerFullName { get; set; } 
    public string? OwnerEmail { get; set; }

    // Ham JSON'u da saklayım; ileride işime yarayabilir

    public string PayloadJson { get; set; } = null!;
}
