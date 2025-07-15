using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SolYSalEcommerce.Services.Interfaces; 
using SolYSalEcommerce.DTOs.Cart;
using SolYSalEcommerce.DTOs.Products;

namespace SolYSalEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService; 

        public CartController(ICartService cartService) 
        {
            _cartService = cartService;
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                throw new UnauthorizedAccessException("ID de usuario no encontrado o inválido en el token.");
            }
            return userId;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                var userId = GetUserId();
                var cartItems = await _cartService.GetCartItemsByUserId(userId);

                var cartItemsDto = cartItems.Select(ci => new
                {
                    CartItemId = ci.Id,
                    ProductVariantId = ci.ProductVariantId,
                    ProductVariant = new ProductVariantDto
                    {
                        Id = ci.ProductVariant.Id,
                        ProductId = ci.ProductVariant.ProductId,
                        Color = ci.ProductVariant.Color,
                        Size = ci.ProductVariant.Size,
                        SKU = ci.ProductVariant.SKU,
                        Stock = ci.ProductVariant.Stock,
                        ImageUrl = ci.ProductVariant.ImageUrl
                    },
                    ProductName = ci.ProductVariant.Product.Name,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.ProductVariant.Product.BasePrice
                });

                return Ok(cartItemsDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddItemToCart([FromBody] AddToCartDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = GetUserId();
                var cartItem = await _cartService.AddToCart(userId, dto);

                if (cartItem == null)
                {
                    return BadRequest(new { Message = "No se pudo agregar el ítem al carrito. Posiblemente stock insuficiente o variante no encontrada." });
                }

                return StatusCode(201, new { Message = "Ítem agregado/actualizado en el carrito.", CartItemId = cartItem.Id });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = GetUserId();
                var cartItem = await _cartService.UpdateCartItemQuantity(userId, dto);

                if (cartItem == null)
                {
                    return NotFound(new { Message = "Ítem no encontrado en el carrito o stock insuficiente para la actualización." });
                }

                return Ok(new { Message = "Cantidad de ítem actualizada.", CartItemId = cartItem.Id });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        [HttpDelete("remove/{productVariantId:guid}")]
        public async Task<IActionResult> RemoveItemFromCart(Guid productVariantId)
        {
            try
            {
                var userId = GetUserId();
                var removed = await _cartService.RemoveCartItem(userId, productVariantId);

                if (!removed)
                {
                    return NotFound(new { Message = "Ítem no encontrado en el carrito." });
                }

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        [HttpPost("clear")]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userId = GetUserId();
                var cleared = await _cartService.ClearCart(userId);

                if (!cleared)
                {
                    return NotFound(new { Message = "El carrito ya está vacío o no se encontraron ítems." });
                }

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }
    }
}