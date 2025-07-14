using AuthApi.Modals;
using AuthApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AddressController : ControllerBase
{
    private readonly AddressService _addressService;

    public AddressController(AddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Address>>> Get() =>
        await _addressService.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Address>> Get(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid address id");
        var address = await _addressService.GetByIdAsync(objectId);
        if (address == null) return NotFound();
        return address;
    }

    [HttpPost]
    public async Task<ActionResult> Create(Address address)
    {
        await _addressService.CreateAsync(address);
        return CreatedAtAction(nameof(Get), new { id = address.Id.ToString() }, address);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, Address addressIn)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid address id");
        var address = await _addressService.GetByIdAsync(objectId);
        if (address == null) return NotFound();
        addressIn.Id = objectId;
        await _addressService.UpdateAsync(objectId, addressIn);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid address id");
        var address = await _addressService.GetByIdAsync(objectId);
        if (address == null) return NotFound();
        await _addressService.DeleteAsync(objectId);
        return NoContent();
    }
}

