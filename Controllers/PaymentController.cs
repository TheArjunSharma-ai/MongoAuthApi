using AuthApi.Modals;
using AuthApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly PaymentService _paymentService;

    public PaymentController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Payment>>> Get() =>
        await _paymentService.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Payment>> Get(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid payment id");
        var payment = await _paymentService.GetByIdAsync(objectId);
        if (payment == null) return NotFound();
        return payment;
    }

    [HttpPost]
    public async Task<ActionResult> Create(Payment payment)
    {
        await _paymentService.CreateAsync(payment);
        return CreatedAtAction(nameof(Get), new { id = payment.Id.ToString() }, payment);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, Payment paymentIn)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid payment id");
        var payment = await _paymentService.GetByIdAsync(objectId);
        if (payment == null) return NotFound();
        paymentIn.Id = objectId;
        await _paymentService.UpdateAsync(objectId, paymentIn);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid payment id");
        var payment = await _paymentService.GetByIdAsync(objectId);
        if (payment == null) return NotFound();
        await _paymentService.DeleteAsync(objectId);
        return NoContent();
    }
}
