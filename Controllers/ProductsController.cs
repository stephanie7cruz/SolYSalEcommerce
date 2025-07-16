using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolYSalEcommerce.DTOs.Products;
using SolYSalEcommerce.Services.Interfaces; 

namespace SolYSalEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService; 

        public ProductsController(IProductService productService) 
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search)
        {
            IEnumerable<ProductDto> products;
            if (!string.IsNullOrWhiteSpace(search))
            {
                products = await _productService.SearchProducts(search);
            }
            else
            {
                products = await _productService.GetAllProducts();
            }
            return Ok(products);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound("Producto no encontrado.");
            }
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // Solo administradores pueden crear productos
        public async Task<IActionResult> Create([FromBody] CreateProductDto createProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _productService.CreateProduct(createProductDto);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")] // Solo administradores pueden actualizar productos
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto updateProductDto) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _productService.UpdateProduct(id, updateProductDto);
            if (product == null)
            {
                return NotFound("Producto no encontrado.");
            }
            return Ok(product);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")] // Solo administradores pueden eliminar productos
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _productService.DeleteProduct(id);
            if (!deleted)
            {
                return NotFound("Producto no encontrado.");
            }
            return NoContent();
        }
    }
}
