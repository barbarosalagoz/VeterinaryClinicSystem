using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using VeterinaryClinic.UI.Models.Appointments;
using VeterinaryClinic.UI.Services;

namespace VeterinaryClinic.UI.Controllers;


[Authorize]

public class AppointmentsController : Controller
{
    private readonly IAppointmentApiClient _appointments;
    private readonly IAnimalApiClient _animals;

    public AppointmentsController(
        IAppointmentApiClient appointments,
        IAnimalApiClient animals)
    {
        _appointments = appointments;
        _animals = animals;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _appointments.GetAppointmentsAsync();
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var animals = await _animals.GetAnimalsAsync();

        var vm = new AppointmentCreateViewModel
        {
            Date = System.DateTime.Today.ToString("yyyy-MM-dd"),
            Time = System.DateTime.Now.ToString("HH:mm"),
            Animals = animals.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = $"{a.Name} ({a.Species})"
            })
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AppointmentCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var animals = await _animals.GetAnimalsAsync();
            model.Animals = animals.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = $"{a.Name} ({a.Species})"
            });

            return View(model);
        }

        await _appointments.CreateAppointmentAsync(model);
        TempData["Success"] = "Appointment created successfully.";

        return RedirectToAction(nameof(Index));
    }
}

