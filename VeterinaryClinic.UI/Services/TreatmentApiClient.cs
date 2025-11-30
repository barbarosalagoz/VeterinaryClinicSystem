using VeterinaryClinic.UI.Models.Treatments;

namespace VeterinaryClinic.UI.Services;

public class TreatmentApiClient : ITreatmentApiClient
{
    private readonly HttpClient _httpClient;

    public TreatmentApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TreatmentViewModel>> GetTreatmentsAsync(int appointmentId)
    {
        // API'den randevuya ait tedavileri çeker
        return await _httpClient.GetFromJsonAsync<List<TreatmentViewModel>>($"treatments?appointmentId={appointmentId}");
    }

    public async Task CreateTreatmentAsync(TreatmentCreateViewModel model)
    {
        // Yeni tedaviyi API'ye gönderir
        await _httpClient.PostAsJsonAsync("treatments", model);
    }
}
