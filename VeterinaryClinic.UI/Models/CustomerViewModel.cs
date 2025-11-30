namespace VeterinaryClinic.UI.Models;

public class CustomerViewModel
{
    public string OwnerName { get; set; } = string.Empty;
    public int PetCount { get; set; }
    public List<string> PetNames { get; set; } = new();

    // Rastgele telefon numarası üretmek için (Görsellik olsun diye)
    public string PhoneNumber { get; set; } = "05" + new Random().Next(30, 55) + " " + new Random().Next(100, 999) + " " + new Random().Next(10, 99) + " " + new Random().Next(10, 99);
}