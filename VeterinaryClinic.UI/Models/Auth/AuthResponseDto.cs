namespace VeterinaryClinic.UI.Models.Auth;

public class AuthResponseDto
{
    public string? Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
}
