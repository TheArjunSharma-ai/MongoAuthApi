using AuthApi.Modals;
using AuthApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly CategoryService _categoryService;

    public CategoryController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Category>>> Get() =>
        await _categoryService.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> Get(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid category id");
        var category = await _categoryService.GetByIdAsync(objectId);
        if (category == null) return NotFound();
        return category;
    }

    [HttpPost]
    public async Task<ActionResult> Create(Category category)
    {
        await _categoryService.CreateAsync(category);
        return CreatedAtAction(nameof(Get), new { id = category.Id.ToString() }, category);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, Category categoryIn)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid category id");
        var category = await _categoryService.GetByIdAsync(objectId);
        if (category == null) return NotFound();
        categoryIn.Id = objectId;
        await _categoryService.UpdateAsync(objectId, categoryIn);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        if (!ObjectId.TryParse(id, out var objectId))
            return BadRequest("Invalid category id");
        var category = await _categoryService.GetByIdAsync(objectId);
        if (category == null) return NotFound();
        await _categoryService.DeleteAsync(objectId);
        return NoContent();
    }
}
