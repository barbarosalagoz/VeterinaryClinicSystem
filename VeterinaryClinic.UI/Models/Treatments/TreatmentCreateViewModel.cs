using System.ComponentModel.DataAnnotations;

namespace VeterinaryClinic.UI.Models.Treatments;

public class TreatmentCreateViewModel
{
    public int AppointmentId { get; set; }

    [Required(ErrorMessage = "Tedavi türü seçiniz.")]
    public string TreatmentType { get; set; }

    public string Notes { get; set; }

    [Required(ErrorMessage = "Maliyet giriniz.")]
    [Range(0, 100000, ErrorMessage = "Geçerli bir tutar giriniz.")]
    public decimal Cost { get; set; }
}