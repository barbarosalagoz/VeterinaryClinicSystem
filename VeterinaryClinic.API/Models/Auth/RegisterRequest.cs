namespace VeterinaryClinic.API.Models.Auth;

public class RegisterRequest
{
    public string? FullName { get; set; }
    public string? Email { get; set; }

    // Düz text gelecek, hashle ve veritabanına yaz

    public string? Password { get; set; }

    // "Müşteri" ya da "Yönetici"
    // Boş gelirse Müşteri kabul et

    public string? Role { get; set; }
}
