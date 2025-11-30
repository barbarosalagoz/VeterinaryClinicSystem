using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VeterinaryClinic.UI.Models;
using VeterinaryClinic.UI.Services;

namespace VeterinaryClinic.UI.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IAnimalApiClient _animalClient;
    private readonly IAppointmentApiClient _appointmentClient;
    private readonly IPaymentApiClient _paymentClient;
    private readonly IWeatherApiClient _weatherClient;

    public HomeController(
        IAnimalApiClient animalClient,
        IAppointmentApiClient appointmentClient,
        IPaymentApiClient paymentClient,
        IWeatherApiClient weatherClient)
    {
        _animalClient = animalClient;
        _appointmentClient = appointmentClient;
        _paymentClient = paymentClient;
        _weatherClient = weatherClient;
    }

    public async Task<IActionResult> Index()
    {
        // 1. Verileri Paralel (Aynı anda) çekiyoruz, hız kazandırır.
        var animalsTask = _animalClient.GetAnimalsAsync();
        var appointmentsTask = _appointmentClient.GetAppointmentsAsync();
        var weatherTask = _weatherClient.GetWeatherAsync("Istanbul"); // Varsayılan şehir: İstanbul

        await Task.WhenAll(animalsTask, appointmentsTask, weatherTask);

        var animals = await animalsTask;
        var appointments = await appointmentsTask;
        var weather = await weatherTask;

        // 2. Modeli dolduruyoruz
        var model = new DashboardViewModel
        {
            TotalAnimals = animals.Count,

            // Tarih kontrolü (Hata vermemesi için string -> DateTime çevrimi)
            // Eğer tarih formatı sorun çıkarırsa try-catch bloğu eklenebilir veya güvenli parse yapılabilir.
            TotalAppointments = appointments.Count(a =>
            {
                if (DateTime.TryParse(a.Date, out var date))
                {
                    return date >= DateTime.Today;
                }
                return false;
            }),

            ActiveVeterinarians = 4, // Sabit veri
            PendingPaymentsAmount = 0, // Henüz API'de yok, 0 geçiyoruz

            // Son 5 hayvanı al
            RecentAnimals = animals.OrderByDescending(a => a.Id).Take(5).ToList(),

            // Hava durumu bilgisini ekle
            Weather = weather
        };

        return View(model);
    }

    public IActionResult Error()
    {
        return View();
    }
}