using AuthApi.Modals;
using AuthApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get() =>
            await _productService.GetAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return BadRequest("Invalid product id");
            var product = await _productService.GetByIdAsync(objectId);
            if (product == null) return NotFound();
            return product;
        }

        [HttpPost]
        public async Task<ActionResult> Create(Product product)
        {
            await _productService.CreateAsync(product);
            return CreatedAtAction(nameof(Get), new { id = product.Id.ToString() }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, Product productIn)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return BadRequest("Invalid product id");
            var product = await _productService.GetByIdAsync(objectId);
            if (product == null) return NotFound();
            productIn.Id = objectId;
            await _productService.UpdateAsync(objectId, productIn);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return BadRequest("Invalid product id");
            var product = await _productService.GetByIdAsync(objectId);
            if (product == null) return NotFound();
            await _productService.DeleteAsync(objectId);
            return NoContent();
        }
    }
}
