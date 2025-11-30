using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VeterinaryClinic.UI.Models.Payments;
using VeterinaryClinic.UI.Services;

namespace VeterinaryClinic.UI.Controllers;

[Authorize]
public class PaymentsController : Controller
{
    private readonly IPaymentApiClient _payments;
    private readonly IAppointmentApiClient _appointments;

    public PaymentsController(
        IPaymentApiClient payments,
        IAppointmentApiClient appointments)
    {
        _payments = payments;
        _appointments = appointments;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int appointmentId)
    {
        // 1. Menüden direkt tıklanırsa randevulara gönder
        if (appointmentId == 0) return RedirectToAction("Index", "Appointments");

        // 2. Randevu Bilgisi
        var appointmentList = await _appointments.GetAppointmentsAsync();
        var appt = appointmentList.FirstOrDefault(a => a.Id == appointmentId);

        if (appt is null) return NotFound("Randevu bulunamadı.");

        // 3. Ödeme Bilgileri (API'den)
        var summary = await _payments.GetSummaryAsync(appointmentId);
        var paymentsList = await _payments.GetPaymentsAsync(appointmentId);

        // 4. Modeli Doldur
        var vm = new PaymentPageViewModel
        {
            AppointmentId = appointmentId,
            AnimalName = appt.AnimalName,
            Species = appt.Species,
            Date = appt.Date,
            Time = appt.Time,

            TotalTreatmentCost = summary?.TotalTreatmentCost ?? 0,
            TotalPaid = summary?.TotalPaid ?? 0,
            RemainingBalance = summary?.RemainingBalance ?? 0,

            // *** KRİTİK KISIM: Sizin Model İsimleriniz ***
            Payments = paymentsList.Select(p => new PaymentListItemViewModel
            {
                Id = p.Id,
                Amount = p.Amount,

                // Burası 'PaidAt' (API'den gelen Date buraya atanıyor)
                PaidAt = p.PaidAt,

                // Burası 'MethodText' (API'den gelen Method string'e çevriliyor)
                MethodText = p.MethodText.ToString()
            }).ToList(),
            // *********************************************

            NewPayment = new PaymentCreateViewModel
            {
                AppointmentId = appointmentId,
                Amount = (summary?.RemainingBalance > 0) ? summary.RemainingBalance : 0,
                Method = PaymentMethod.Cash,

                // Dropdown Listesini Doldurma
                PaymentMethods = Enum.GetValues(typeof(PaymentMethod))
                                     .Cast<PaymentMethod>()
                                     .Select(e => new SelectListItem
                                     {
                                         Text = e.ToString(),
                                         Value = e.ToString()
                                     })
                                     .ToList()
            }
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PaymentPageViewModel page)
    {
        var model = page.NewPayment;

        // Validasyon hatası varsa (örn: Tutar 0 ise) sayfayı tekrar doldurup göster
        if (!ModelState.IsValid)
        {
            return await Index(model.AppointmentId);
        }

        await _payments.CreatePaymentAsync(model);

        TempData["Success"] = "Ödeme başarıyla alındı.";
        return RedirectToAction(nameof(Index), new { appointmentId = model.AppointmentId });
    }
}