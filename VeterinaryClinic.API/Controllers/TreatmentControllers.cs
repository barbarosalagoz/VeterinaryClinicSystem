using Microsoft.AspNetCore.Mvc;
using VeterinaryClinic.Business.Interfaces;
using VeterinaryClinic.Entities;


namespace VeterinaryClinic.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TreatmentsController : ControllerBase
{
    private readonly ITreatmentService _treatmentService;

    public TreatmentsController(ITreatmentService treatmentService)
    {
        _treatmentService = treatmentService;
    }

    // GET: api/Treatment
    [HttpGet("by-appointment/{appointmentId:int}")]
    public async Task<ActionResult<IEnumerable<Treatment>>> GetByAppointment(int appointmentId)
    {
        var treatments = await _treatmentService.GetByAppointmentIdAsync(appointmentId);
        return Ok(treatments);
    }

    // GET: api/Treatments/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Treatment>> GetById(int id)
    {
        var treatment = await _treatmentService.GetByIdAsync(id);

        if (treatment == null)
        {
            return NotFound();
        }
        return Ok(treatment);
    }

    // POST: api/Treatment
    [HttpPost]
    public async Task<ActionResult<Treatment>> Create([FromBody] Treatment treatment)
    {
        var created = await _treatmentService.CreateAsync(treatment);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}
