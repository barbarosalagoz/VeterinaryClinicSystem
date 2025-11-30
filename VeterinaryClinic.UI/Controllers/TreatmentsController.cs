using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeterinaryClinic.UI.Models.Treatments;
using VeterinaryClinic.UI.Services;

namespace VeterinaryClinic.UI.Controllers;

[Authorize]
public class TreatmentsController : Controller
{
    private readonly ITreatmentApiClient _treatmentClient;

    public TreatmentsController(ITreatmentApiClient treatmentClient)
    {
        _treatmentClient = treatmentClient;
    }

    // GET: Tedavi Ekleme Sayfası
    [HttpGet]
    public IActionResult Create(int appointmentId)
    {
        // Formu açarken randevu ID'sini modele koyup gönderiyoruz ki hangi randevuya tedavi eklediğimizi bilelim
        var model = new TreatmentCreateViewModel
        {
            AppointmentId = appointmentId
        };
        return View(model);
    }

    // POST: Tedavi Kaydetme İşlemi
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TreatmentCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Servis aracılığıyla API'ye gönder
        await _treatmentClient.CreateTreatmentAsync(model);

        TempData["Success"] = "Tedavi başarıyla eklendi.";

        // İşlem bitince Ödeme sayfasına geri dönelim ki eklenen tutarı orada görelim
        return RedirectToAction("Index", "Payments", new { appointmentId = model.AppointmentId });
    }
}