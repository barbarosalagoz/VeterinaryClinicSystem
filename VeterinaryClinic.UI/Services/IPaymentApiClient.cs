using System.Collections.Generic;
using System.Threading.Tasks;
using VeterinaryClinic.UI.Models.Payments;

namespace VeterinaryClinic.UI.Services
{
    public interface IPaymentApiClient
    {
        // Randevuya ait özet: Toplam tedavi ücreti, toplam ödeme, kalan bakiye
        Task<AppointmentPaymentSummaryViewModel?> GetSummaryAsync(int appointmentId);

        // Randevuya ait tek tek ödemeler
        Task<IReadOnlyList<PaymentListItemViewModel>> GetPaymentsAsync(int appointmentId);

        // Yeni ödeme ekleme
        Task CreatePaymentAsync(PaymentCreateViewModel model);
    }
}
