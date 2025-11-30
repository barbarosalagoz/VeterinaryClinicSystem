using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeterinaryClinic.UI.Services;
using VeterinaryClinic.UI.Models; // Modellerin olduğu namespace

namespace VeterinaryClinic.UI.Controllers;

// [ApiController]  <-- SİL (Bu bir web sayfası, API değil)
// [Route("api/[controller]")] <-- SİL (URL /Animals/Index olmalı)
[Authorize] // Sadece giriş yapanlar görebilsin
public class AnimalsController : Controller
{
    private readonly IAnimalApiClient _animalApiClient;

    public AnimalsController(IAnimalApiClient animalApiClient)
    {
        _animalApiClient = animalApiClient;
    }

    // GET: /Animals
    public async Task<IActionResult> Index()
    {
        // Backend API'den veriyi çeker
        var animals = await _animalApiClient.GetAnimalsAsync();

        // Views/Animals/Index.cshtml sayfasına gönderir
        return View(animals);
    }
}