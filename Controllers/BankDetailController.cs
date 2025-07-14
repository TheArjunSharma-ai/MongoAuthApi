using AuthApi.Modals;
using AuthApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BankDetailController : ControllerBase
{
    private readonly BankDetailService _bankDetailService;

    public BankDetailController(BankDetailService bankDetailService)
    {
        _bankDetailService = bankDetailService;
    }

    [HttpGet]
    public async Task<ActionResult<List<BankDetail>>> Get() =>
        await _bankDetailService.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<BankDetail>> Get(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid bank detail id");
        var bankDetail = await _bankDetailService.GetByIdAsync(objectId);
        if (bankDetail == null) return NotFound();
        return bankDetail;
    }

    [HttpPost]
    public async Task<ActionResult> Create(BankDetail bankDetail)
    {
        await _bankDetailService.CreateAsync(bankDetail);
        return CreatedAtAction(nameof(Get), new { id = bankDetail.Id.ToString() }, bankDetail);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, BankDetail bankDetailIn)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid bank detail id");
        var bankDetail = await _bankDetailService.GetByIdAsync(objectId);
        if (bankDetail == null) return NotFound();
        bankDetailIn.Id = objectId;
        await _bankDetailService.UpdateAsync(objectId, bankDetailIn);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid bank detail id");
        var bankDetail = await _bankDetailService.GetByIdAsync(objectId);
        if (bankDetail == null) return NotFound();
        await _bankDetailService.DeleteAsync(objectId);
        return NoContent();
    }
}
