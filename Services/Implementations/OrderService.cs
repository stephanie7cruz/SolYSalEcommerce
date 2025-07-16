// Services/Implementations/OrderService.cs
using Microsoft.EntityFrameworkCore;
using SolYSalEcommerce.Data;
using SolYSalEcommerce.Models;
using SolYSalEcommerce.Services.Interfaces;
using SolYSalEcommerce.DTOs.Orders; // Importar los DTOs de órdenes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SolYSalEcommerce.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderService> _logger;

        public OrderService(ApplicationDbContext context, ILogger<OrderService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<OrderDto>> GetMyOrdersAsync(Guid userId)
        {
            var orders = await _context.Orders
                                       .Where(o => o.UserId == userId)
                                       .Include(o => o.OrderDetails)
                                           .ThenInclude(od => od.ProductVariant)
                                               .ThenInclude(pv => pv.Product) // Necesario para ProductName y BasePrice
                                       .OrderByDescending(o => o.OrderDate)
                                       .ToListAsync();

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                UserId = o.UserId,
                OrderNumber = o.OrderNumber,
                CreatedAt = o.OrderDate,
                TotalAmount = o.TotalPrice,
                Status = o.OrderStatus,
                OrderDetails = o.OrderDetails.Select(od => new OrderDetailDto
                {
                    Id = od.Id,
                    OrderId = od.OrderId, // <-- CORREGIDO: Asigna el OrderId real del detalle
                    ProductVariantId = od.ProductVariantId,
                    Quantity = od.Quantity,
                    UnitPrice = od.PricePerUnit,
                    ProductVariantDetails = new ProductVariantInOrderDto
                    {
                        Id = od.ProductVariant.Id,
                        ProductId = od.ProductVariant.ProductId, // <-- CORREGIDO: Asigna el ProductId real
                        Sku = od.ProductVariant.SKU,
                        Color = od.ProductVariant.Color,
                        Size = od.ProductVariant.Size,
                        ImageUrl = od.ProductVariant.ImageUrl,
                        BasePrice = od.ProductVariant.BasePrice,
                        ProductName = od.ProductVariant.Product?.Name ?? "Producto Desconocido" // Uso de null-conditional operator
                    }
                }).ToList()
            }).ToList();
        }

        public async Task<OrderDto?> GetOrderByIdAsync(Guid orderId, Guid userId)
        {
            var order = await _context.Orders
                                      .Include(o => o.OrderDetails)
                                          .ThenInclude(od => od.ProductVariant)
                                              .ThenInclude(pv => pv.Product) // Necesario para ProductName y BasePrice
                                      .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
            {
                return null;
            }

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderNumber = order.OrderNumber,
                CreatedAt = order.OrderDate,
                TotalAmount = order.TotalPrice,
                Status = order.OrderStatus,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailDto
                {
                    Id = od.Id,
                    OrderId = od.OrderId, // <-- CORREGIDO: Asigna el OrderId real del detalle
                    ProductVariantId = od.ProductVariantId,
                    Quantity = od.Quantity,
                    UnitPrice = od.PricePerUnit,
                    ProductVariantDetails = new ProductVariantInOrderDto
                    {
                        Id = od.ProductVariant.Id,
                        ProductId = od.ProductVariant.ProductId, // <-- CORREGIDO: Asigna el ProductId real
                        Sku = od.ProductVariant.SKU,
                        Color = od.ProductVariant.Color,
                        Size = od.ProductVariant.Size,
                        ImageUrl = od.ProductVariant.ImageUrl,
                        BasePrice = od.ProductVariant.BasePrice,
                        ProductName = od.ProductVariant.Product?.Name ?? "Producto Desconocido" // Uso de null-conditional operator
                    }
                }).ToList()
            };
        }

        public async Task<CreateOrderResponseDto?> CreateOrderFromCartAsync(Guid userId)
        {
            var cartItems = await _context.CartItems
                                          .Where(ci => ci.UserId == userId)
                                          .Include(ci => ci.ProductVariant)
                                              .ThenInclude(pv => pv.Product) // Incluir Product para ProductName y BasePrice
                                          .ToListAsync();

            if (!cartItems.Any())
            {
                _logger.LogWarning("Attempt to create order from empty cart for user {UserId}.", userId);
                return null;
            }

            // Verificar stock antes de crear el pedido y también el BasePrice
            foreach (var item in cartItems)
            {
                if (item.ProductVariant == null)
                {
                    _logger.LogError("ProductVariant not found for cart item {CartItemId} for user {UserId}.", item.Id, userId);
                    throw new InvalidOperationException($"Product variant for cart item {item.Id} not found.");
                }
                if (item.ProductVariant.Stock < item.Quantity)
                {
                    _logger.LogWarning("Insufficient stock for product variant {SKU} (Available: {AvailableStock}, Requested: {RequestedQuantity}) for user {UserId}.",
                        item.ProductVariant.SKU, item.ProductVariant.Stock, item.Quantity, userId);
                    throw new InvalidOperationException($"Insufficient stock for {item.ProductVariant.SKU}. Available: {item.ProductVariant.Stock}, Requested: {item.Quantity}");
                }
                // Añadir una verificación de precio aquí.
                if (item.ProductVariant.BasePrice <= 0) // Considera si 0 es un precio válido en tu negocio
                {
                    _logger.LogError("ProductVariant {SKU} has invalid BasePrice ({BasePrice}) for cart item {CartItemId} for user {UserId}.",
                        item.ProductVariant.SKU, item.ProductVariant.BasePrice, item.Id, userId);
                    throw new InvalidOperationException($"Product variant {item.ProductVariant.SKU} has an invalid price. Cannot create order.");
                }
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newOrder = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    TotalPrice = 0, // Se calculará después de agregar los detalles
                    OrderStatus = "Pending",
                    OrderNumber = GenerateOrderNumber()
                };
                _context.Orders.Add(newOrder);
                await _context.SaveChangesAsync(); // Guarda el pedido para obtener su ID

                decimal totalOrderPrice = 0;
                foreach (var item in cartItems)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = newOrder.Id, // <-- Asegura que se usa el ID de la nueva orden
                        ProductVariantId = item.ProductVariantId,
                        Quantity = item.Quantity,
                        PricePerUnit = item.ProductVariant.BasePrice // Asume BasePrice es el precio por unidad
                    };
                    _context.OrderDetails.Add(orderDetail);
                    totalOrderPrice += orderDetail.Quantity * orderDetail.PricePerUnit;

                    // Reducir el stock de la variante
                    item.ProductVariant.Stock -= item.Quantity;
                    _context.ProductVariants.Update(item.ProductVariant);
                }

                newOrder.TotalPrice = totalOrderPrice; // Asigna el total calculado
                _context.Orders.Update(newOrder);

                // Eliminar ítems del carrito después de crear el pedido
                _context.CartItems.RemoveRange(cartItems);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new CreateOrderResponseDto
                {
                    OrderId = newOrder.Id,
                    OrderNumber = newOrder.OrderNumber,
                    TotalAmount = newOrder.TotalPrice,
                    CreatedAt = newOrder.OrderDate,
                    Status = newOrder.OrderStatus,
                    WompiCheckoutUrl = "" // Asegúrate de rellenar esto si usas Wompi
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating order for user {UserId}", userId);
                throw;
            }
        }

        private string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
        }
    }
}