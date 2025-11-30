using Microsoft.AspNetCore.Mvc;
using VeterinaryClinic.Business.Interfaces;
using VeterinaryClinic.Entities;
using Microsoft.AspNetCore.Authorization;

namespace VeterinaryClinic.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    // Şimdi sırada metotlar var
    // GET: api/appointments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetAll()
    {
        var appointments = await _appointmentService.GetAllAsync();
        return Ok(appointments);
    }

    // GET: api/appointments/{id}
    [HttpGet("{id:int}")]

    public async Task<ActionResult<Appointment>> GetById(int id)
    {
        var appointment = await _appointmentService.GetByIdAsync(id);

        if (appointment == null)
        {
            return NotFound();
        }
        return Ok(appointment);
    }

    // POST: api/appointments -> Yeni randevu oluşturma
    [HttpPost]
    public async Task<ActionResult<Appointment>> Create([FromBody] Appointment appointment)
    {
        var created = await _appointmentService.CreateAsync(appointment);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}
