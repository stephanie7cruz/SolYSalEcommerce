using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Para ILogger
using SolYSalEcommerce.Models; // Asegúrate de que este using esté presente para tus modelos
using SolYSalEcommerce.Services.Interfaces; // Para IOrderService
using System.Security.Claims; // Necesario para obtener el UserId

namespace SolYSalEcommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize] // Descomenta si usas autenticación y quieres que solo usuarios autenticados puedan acceder a estas rutas
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger; // Para loguear errores

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        // Helper para obtener el ID de usuario
        private Guid? GetUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                return null;
            }
            return userId;
        }

        // GET: api/Orders/MyOrders
        [HttpGet("MyOrders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetMyOrders()
        {
            var userId = GetUserId();
            if (!userId.HasValue)
            {
                return Unauthorized("User is not authenticated or user ID is invalid.");
            }

            var orders = await _orderService.GetMyOrdersAsync(userId.Value);

            if (!orders.Any())
            {
                return NotFound("No orders found for this user.");
            }

            return Ok(orders);
        }

        // GET: api/Orders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var userId = GetUserId();
            if (!userId.HasValue)
            {
                return Unauthorized("User is not authenticated or user ID is invalid.");
            }

            var order = await _orderService.GetOrderByIdAsync(id, userId.Value);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder()
        {
            var userId = GetUserId();
            if (!userId.HasValue)
            {
                return Unauthorized("User is not authenticated or user ID is invalid.");
            }

            try
            {
                // El servicio se encarga de toda la lógica, incluyendo la verificación de stock y la creación de la orden.
                // No hay llamadas a pasarelas de pago aquí.
                var order = await _orderService.CreateOrderFromCartAsync(userId.Value);

                if (order == null)
                {
                    // Esto significa que el carrito estaba vacío o hubo algún otro problema manejado en el servicio
                    return BadRequest("Failed to create order. Cart might be empty or invalid.");
                }

                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            catch (InvalidOperationException ex)
            {
                // Captura la excepción de stock insuficiente o similar lanzada por el servicio
                _logger.LogWarning("Order creation failed due to invalid operation for user {UserId}: {Message}", userId.Value, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating order for user {UserId}", userId.Value);
                return StatusCode(500, "Internal server error while processing your request.");
            }
        }
    }
}