using VeterinaryClinic.UI.Models.Treatments;

namespace VeterinaryClinic.UI.Services;

public interface ITreatmentApiClient
{
    // Bir randevuya ait tedavileri getir
    Task<List<TreatmentViewModel>> GetTreatmentsAsync(int appointmentId);

    // Yeni tedavi ekle
    Task CreateTreatmentAsync(TreatmentCreateViewModel model);
}