namespace VeterinaryClinic.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Soft delete
    public bool IsDeleted { get; set; }
}
