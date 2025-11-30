using VeterinaryClinic.UI.Models.Appointments;

namespace VeterinaryClinic.UI.Services
{
    public class AppointmentApiClient : IAppointmentApiClient
    {
        private readonly HttpClient _httpClient;

        public AppointmentApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyList<AppointmentListItemViewModel>> GetAppointmentsAsync()
        {
            // 1) Randevuları çek
            var appointments = await _httpClient
                .GetFromJsonAsync<List<AppointmentDto>>("api/Appointments")
                ?? new List<AppointmentDto>();

            // 2) Hayvanları çek
            var animals = await _httpClient
                .GetFromJsonAsync<List<AnimalDto>>("api/Animals")
                ?? new List<AnimalDto>();

            // 3) Id -> Animal lookup
            var animalLookup = animals.ToDictionary(a => a.Id);

            // 4) Join + ViewModel’e map et
            var result = appointments.Select(a =>
            {
                animalLookup.TryGetValue(a.AnimalId, out var animal);

                var animalName = animal?.Name ?? $"#{a.AnimalId}";
                var species = animal?.Species ?? string.Empty;

                var statusText = a.Status switch
                {
                    1 => "Scheduled",
                    2 => "Completed",
                    3 => "Cancelled",
                    _ => "Unknown"
                };

                return new AppointmentListItemViewModel
                {
                    Id = a.Id,
                    AnimalName = animalName,
                    Species = species,
                    Date = a.Date,
                    Time = a.Time,
                    StatusText = statusText
                };
            }).ToList();

            return result;
        }

        public async Task CreateAppointmentAsync(AppointmentCreateViewModel model)
        {
            var payload = new
            {
                animalId = model.AnimalId,
                date = model.Date,
                time = model.Time,
                notes = model.Notes
            };

            var response = await _httpClient.PostAsJsonAsync("api/Appointments", payload);
            response.EnsureSuccessStatusCode();
        }

        // API’den gelen ham DTO (Swagger’daki JSON’a birebir uygun)
        private sealed class AppointmentDto
        {
            public int Id { get; set; }
            public int AnimalId { get; set; }
            public string Date { get; set; } = "";
            public string Time { get; set; } = "";
            public int Status { get; set; }
        }

        // api/Animals’dan gelecek minimal DTO
        private sealed class AnimalDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public string Species { get; set; } = "";
        }
    }
}
