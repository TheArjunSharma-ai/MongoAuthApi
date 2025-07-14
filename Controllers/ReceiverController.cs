using AuthApi.Modals;
using AuthApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ReceiverController : ControllerBase
{
    private readonly ReceiverService _receiverService;

    public ReceiverController(ReceiverService receiverService)
    {
        _receiverService = receiverService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Receiver>>> Get() =>
        await _receiverService.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Receiver>> Get(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid receiver id");
        var receiver = await _receiverService.GetByIdAsync(objectId);
        if (receiver == null) return NotFound();
        return receiver;
    }

    [HttpPost]
    public async Task<ActionResult> Create(Receiver receiver)
    {
        await _receiverService.CreateAsync(receiver);
        return CreatedAtAction(nameof(Get), new { id = receiver.Id.ToString() }, receiver);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, Receiver receiverIn)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid receiver id");
        var receiver = await _receiverService.GetByIdAsync(objectId);
        if (receiver == null) return NotFound();
        receiverIn.Id = objectId;
        await _receiverService.UpdateAsync(objectId, receiverIn);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid receiver id");
        var receiver = await _receiverService.GetByIdAsync(objectId);
        if (receiver == null) return NotFound();
        await _receiverService.DeleteAsync(objectId);
        return NoContent();
    }
}
