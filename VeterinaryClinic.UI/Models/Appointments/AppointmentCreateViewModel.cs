using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VeterinaryClinic.UI.Models.Appointments;

public class AppointmentCreateViewModel
{
    [Display(Name = "Animal")]
    [Required]

    public int AnimalId { get; set; }

    [Required]
    [Display(Name = "Date")]
    public string Date { get; set; } = "";

    [Required]
    [Display(Name = "Time")]
    public string Time { get; set; } = "";

    [Display(Name = "Notes")]
    public string? Notes { get; set; }

    // Dropdown

    public IEnumerable<SelectListItem>? Animals { get; set; }
}
