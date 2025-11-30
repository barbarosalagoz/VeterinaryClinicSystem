namespace VeterinaryClinic.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public UserRole Role { get; set; }

    // Navigasyon
    public ICollection<Animal> Animals { get; set; } = new List<Animal>();
}
