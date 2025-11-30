using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeterinaryClinic.UI.Models;
using VeterinaryClinic.UI.Services;

namespace VeterinaryClinic.UI.Controllers;

[Authorize]
public class CustomersController : Controller
{
    private readonly IAnimalApiClient _animalClient;

    public CustomersController(IAnimalApiClient animalClient)
    {
        _animalClient = animalClient;
    }

    public async Task<IActionResult> Index()
    {
        // API'den tüm hayvanları çek
        var animals = await _animalClient.GetAnimalsAsync();

        // Hayvanları "Sahip Adı"na (OwnerName) göre grupla = Müşteri Listesi
        var customers = animals
            .GroupBy(a => a.OwnerName)
            .Select(g => new CustomerViewModel
            {
                OwnerName = string.IsNullOrWhiteSpace(g.Key) ? "Bilinmeyen Müşteri" : g.Key,
                PetCount = g.Count(),
                PetNames = g.Select(x => x.Name).ToList()
            })
            .OrderBy(c => c.OwnerName)
            .ToList();

        return View(customers);
    }
}
