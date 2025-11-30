using System.Net.Http;
using System.Net.Http.Json;
using VeterinaryClinic.UI.Models.Payments;

namespace VeterinaryClinic.UI.Services
{
    public class PaymentApiClient : IPaymentApiClient
    {
        private readonly HttpClient _httpClient;

        public PaymentApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Ödeme listesi
        public async Task<IReadOnlyList<PaymentListItemViewModel>> GetPaymentsAsync(int appointmentId)
        {
            var payments = await _httpClient
                .GetFromJsonAsync<List<PaymentListItemDto>>(
                    $"api/Payments?appointmentId={appointmentId}");

            return payments?
                       .Select(p => new PaymentListItemViewModel
                       {
                           Id = p.Id,
                           Amount = p.Amount,
                           PaidAt = p.PaidAt,
                           MethodText = p.Method.ToString(),
                           Notes = p.Notes   // İstersen burayı da kaldırabilirsin
                       })
                       .ToList()
                   ?? new List<PaymentListItemViewModel>();
        }

        // Özet
        public async Task<AppointmentPaymentSummaryViewModel?> GetSummaryAsync(int appointmentId)
        {
            var dto = await _httpClient
                .GetFromJsonAsync<PaymentSummaryDto>(
                    $"api/Payments/summary?appointmentId={appointmentId}");

            if (dto is null)
                return null;

            return new AppointmentPaymentSummaryViewModel
            {
                AppointmentId = dto.AppointmentId,
                TotalTreatmentCost = dto.TotalTreatmentCost,
                TotalPaid = dto.TotalPaid,
                RemainingBalance = dto.RemainingBalance
            };
        }

        // Ödeme oluştur
        public async Task CreatePaymentAsync(PaymentCreateViewModel model)
        {
            var dto = new PaymentCreateDto
            {
                AppointmentId = model.AppointmentId,
                Amount = model.Amount,
                Method = model.Method,
                Notes = model.Notes
            };

            var response = await _httpClient.PostAsJsonAsync("api/Payments", dto);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"Ödeme oluşturulamadı. Status: {(int)response.StatusCode} - {response.StatusCode}\n" +
                    $"Body: {body}");
            }
        }
    }
}
