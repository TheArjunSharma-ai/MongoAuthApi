using AuthApi.Modals;
using AuthApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly CartService _cartService;

    public CartController(CartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Cart>>> Get() =>
        await _cartService.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Cart>> Get(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid cart id");
        var cart = await _cartService.GetByIdAsync(objectId);
        if (cart == null) return NotFound();
        return cart;
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<Cart>>> GetByUser(string userId)
    {
        if (!ObjectId.TryParse(userId, out var objectId))
            return BadRequest("Invalid user id");
        return await _cartService.GetByUserIdAsync(objectId);
    }

    [HttpPost]
    public async Task<ActionResult> Create(Cart cart)
    {
        await _cartService.CreateAsync(cart);
        return CreatedAtAction(nameof(Get), new { id = cart.Id.ToString() }, cart);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, Cart cartIn)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid cart id");
        var cart = await _cartService.GetByIdAsync(objectId);
        if (cart == null) return NotFound();
        cartIn.Id = objectId;
        await _cartService.UpdateAsync(objectId, cartIn);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid cart id");
        var cart = await _cartService.GetByIdAsync(objectId);
        if (cart == null) return NotFound();
        await _cartService.DeleteAsync(objectId);
        return NoContent();
    }
}
