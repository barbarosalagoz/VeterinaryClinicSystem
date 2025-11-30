using System.ComponentModel.DataAnnotations;

namespace VeterinaryClinic.UI.Models.Auth;

public class LoginViewModel
{
    [Required, DataType(DataType.Password)]
    public string? Password { get; set; }

    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}
