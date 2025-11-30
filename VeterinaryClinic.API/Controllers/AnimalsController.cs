using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VeterinaryClinic.Business.Interfaces;
using VeterinaryClinic.Entities;

namespace VeterinaryClinic.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalService _animalService;
    
    public AnimalsController(IAnimalService animalService)
    {
        _animalService = animalService;
    }

    // GET: api/Animals
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Animal>>> GetAll()
    {
        var animals = await _animalService.GetAllAsync();
        return Ok(animals);
    }

    // GET: api/Animals/5
    [HttpGet("{id:int}")]

    public async Task<ActionResult<Animal>> GetById(int id)
    {
        var animal = await _animalService.GetByIdAsync(id);

        if (animal is null)
        {
            return NotFound();
        }
        return Ok(animal);
    }

    // POST: api/Animals

    [HttpPost]
    public async Task<ActionResult<Animal>> Create([FromBody]Animal animal)
    {
        var created = await _animalService.CreateAsync(animal);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}
