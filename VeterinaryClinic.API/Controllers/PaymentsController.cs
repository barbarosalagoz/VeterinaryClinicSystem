using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeterinaryClinic.Business.Interfaces;
using VeterinaryClinic.DataAccess.Context;
using VeterinaryClinic.Entities;

namespace VeterinaryClinic.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // İstersen kaldırabilirsin ama JWT ile korumak mantıklı
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly VeterinaryClinicDbContext _dbContext;

    public PaymentsController(
        IPaymentService paymentService,
        VeterinaryClinicDbContext dbContext)
    {
        _paymentService = paymentService;
        _dbContext = dbContext;
    }

    /// <summary>
    /// Belirli bir randevuya ait ödeme listesini döner.
    /// GET /api/payments?appointmentId=5
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaymentDto>>> GetByAppointment([FromQuery] int appointmentId)
    {
        if (appointmentId <= 0)
            return BadRequest("appointmentId must be greater than zero.");

        var payments = await _paymentService.GetByAppointmentIdAsync(appointmentId);

        var result = payments
            .OrderBy(p => p.PaymentDate)
            .Select(p => new PaymentDto
            {
                Id = p.Id,
                AppointmentId = p.AppointmentId,
                Amount = p.AmountPaid,
                Method = p.PaymentMethod.ToString(),
                Date = p.PaymentDate
            })
            .ToList();

        return Ok(result);
    }

    /// <summary>
    /// Tek bir ödeme detayını döner.
    /// GET /api/payments/10
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PaymentDto>> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid id.");

        var payment = await _paymentService.GetByIdAsync(id);

        if (payment is null)
            return NotFound();

        var dto = new PaymentDto
        {
            Id = payment.Id,
            AppointmentId = payment.AppointmentId,
            Amount = payment.AmountPaid,
            Method = payment.PaymentMethod.ToString(),
            Date = payment.PaymentDate
        };

        return Ok(dto);
    }

    /// <summary>
    /// Belirli bir randevuya ait toplam tedavi ücreti, toplam ödeme ve kalan bakiyeyi döner.
    /// GET /api/payments/summary?appointmentId=5
    /// </summary>
    [HttpGet("summary")]
    public async Task<ActionResult<PaymentSummaryDto>> GetSummary([FromQuery] int appointmentId)
    {
        if (appointmentId <= 0)
            return BadRequest("appointmentId must be greater than zero.");

        // Toplam tedavi maliyeti (Treatment.Cost)
        var totalTreatmentCost = await _dbContext.Treatments
            .Where(t => t.AppointmentId == appointmentId && !t.IsDeleted)
            .SumAsync(t => (decimal?)t.Cost) ?? 0m;

        // Toplam ödenen (Payments.AmountPaid)
        var totalPaid = await _dbContext.Payments
            .Where(p => p.AppointmentId == appointmentId && !p.IsDeleted)
            .SumAsync(p => (decimal?)p.AmountPaid) ?? 0m;

        var remaining = totalTreatmentCost - totalPaid;
        if (remaining < 0)
            remaining = 0;

        var dto = new PaymentSummaryDto
        {
            AppointmentId = appointmentId,
            TotalTreatmentCost = totalTreatmentCost,
            TotalPaid = totalPaid,
            RemainingBalance = remaining
        };

        return Ok(dto);
    }

    /// <summary>
    /// Yeni ödeme oluşturur.
    /// POST /api/payments
    /// Body: { "appointmentId": 5, "amount": 300, "method": 1, "notes": "Nakit" }
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<object>> Create([FromBody] CreatePaymentRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (request.AppointmentId <= 0)
            return BadRequest("AppointmentId is required.");

        // Appointment var mı kontrol edelim (hata mesajı daha anlamlı olsun)
        var appointmentExists = await _dbContext.Appointments
            .AnyAsync(a => a.Id == request.AppointmentId && !a.IsDeleted);

        if (!appointmentExists)
            return NotFound($"Appointment with id {request.AppointmentId} not found.");

        var payment = new Payment
        {
            AppointmentId = request.AppointmentId,
            AmountPaid = request.Amount,
            PaymentDate = DateTime.UtcNow,
            PaymentMethod = request.Method
            // Notes property'in yoksa eklemiyoruz;
            // varsa buraya: Notes = request.Notes
        };

        var created = await _paymentService.CreateAsync(payment);

        // CreatedAtAction dememize gerek yok aslında ama REST açısından hoş:
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, new { id = created.Id });
    }
}

/// <summary>
/// API'den dönen ödeme DTO'su (UI'daki PaymentApiClient ile eşleşecek şekilde).
/// </summary>
public sealed class PaymentDto
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }

    public decimal Amount { get; set; }
    public string Method { get; set; } = string.Empty;

    public DateTime Date { get; set; }
}

/// <summary>
/// Ödeme özeti DTO'su – UI'deki Summary için.
/// </summary>
public sealed class PaymentSummaryDto
{
    public int AppointmentId { get; set; }

    public decimal TotalTreatmentCost { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal RemainingBalance { get; set; }
}

/// <summary>
/// UI'dan gelen ödeme oluşturma isteği.
/// </summary>
public sealed class CreatePaymentRequest
{
    public int AppointmentId { get; set; }
    public decimal Amount { get; set; }

    public PaymentMethod Method { get; set; }

    // Şimdilik opsiyonel, Payment entity'de Note yoksa DB'ye gitmeyecek
    public string? Notes { get; set; }
}
