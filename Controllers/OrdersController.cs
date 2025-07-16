using Microsoft.AspNetCore.Mvc;
using SolYSalEcommerce.Services.Interfaces;
using SolYSalEcommerce.DTOs.Orders;
using System.Security.Claims;
using System; // Para Guid y Exception
using System.Collections.Generic; // Para IEnumerable
using System.Threading.Tasks; // Para Task
using Microsoft.Extensions.Logging; // <--- ¡Asegúrate de que esta línea esté aquí!

namespace SolYSalEcommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize(Roles = "Client,Admin")] // Asegúrate de la autorización si la tienes
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger; // <--- ¡DECLARACIÓN DEL LOGGER AQUÍ!

        // Constructor con inyección de IOrderService y ILogger
        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger; // <--- ASIGNACIÓN DEL LOGGER AQUÍ!
        }

        [HttpGet]
        // [Authorize(Roles = "Client")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetUserOrders()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                _logger.LogWarning("Unauthorized attempt to get user orders: User ID not found or invalid in token.");
                return Unauthorized("User ID not found in token.");
            }

            try
            {
                var orders = await _orderService.GetMyOrdersAsync(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders for user {UserId}", userId);
                return StatusCode(500, new { message = "Ocurrió un error interno al obtener las órdenes." });
            }
        }

        [HttpGet("{orderId}")]
        // [Authorize(Roles = "Client")]
        public async Task<ActionResult<OrderDto>> GetOrderById(Guid orderId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                _logger.LogWarning("Unauthorized attempt to get order {OrderId}: User ID not found or invalid in token.", orderId);
                return Unauthorized("User ID not found in token.");
            }

            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId, userId);
                if (order == null)
                {
                    _logger.LogInformation("Order {OrderId} not found or not accessible for user {UserId}.", orderId, userId);
                    return NotFound("Order not found or not accessible to this user.");
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order {OrderId} for user {UserId}.", orderId, userId);
                return StatusCode(500, new { message = "Ocurrió un error interno al obtener la orden." });
            }
        }

        [HttpPost]
        // [Authorize(Roles = "Client")]
        public async Task<ActionResult<CreateOrderResponseDto>> CreateOrder()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                _logger.LogWarning("Unauthorized attempt to create order: User ID not found or invalid in token.");
                return Unauthorized("User ID not found in token.");
            }

            try
            {
                var createdOrderDto = await _orderService.CreateOrderFromCartAsync(userId);
                if (createdOrderDto == null)
                {
                    _logger.LogWarning("Failed to create order for user {UserId}: Cart was empty.", userId);
                    return BadRequest(new { message = "El carrito está vacío. No se puede crear una orden." });
                }

                // Ajusta el nameof(GetOrderById) para que el método tenga un parámetro {orderId}
                // Si el parámetro se llama 'id', entonces sería new { id = createdOrderDto.OrderId }
                return CreatedAtAction(nameof(GetOrderById), new { orderId = createdOrderDto.OrderId }, createdOrderDto);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Failed to create order for user {UserId} due to invalid operation: {Message}", userId, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating order for user {UserId}.", userId);
                return StatusCode(500, new { message = "Ocurrió un error interno al crear la orden." });
            }
        }
    }
}