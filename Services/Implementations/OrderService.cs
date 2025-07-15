// Services/Implementations/OrderService.cs
using Microsoft.EntityFrameworkCore;
using SolYSalEcommerce.Data;
using SolYSalEcommerce.Models;
using SolYSalEcommerce.Services.Interfaces;
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

        public async Task<IEnumerable<Order>> GetMyOrdersAsync(Guid userId)
        {
            var orders = await _context.Orders
                                       .Where(o => o.UserId == userId)
                                       .Include(o => o.OrderDetails)
                                           .ThenInclude(od => od.ProductVariant)
                                               .ThenInclude(pv => pv.Product)
                                       .OrderByDescending(o => o.OrderDate)
                                       .ToListAsync();
            return orders;
        }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId, Guid userId)
        {
            var order = await _context.Orders
                                      .Include(o => o.OrderDetails)
                                          .ThenInclude(od => od.ProductVariant)
                                              .ThenInclude(pv => pv.Product)
                                      .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            return order;
        }

        public async Task<Order?> CreateOrderFromCartAsync(Guid userId)
        {
            var cartItems = await _context.CartItems
                                          .Where(ci => ci.UserId == userId)
                                          .Include(ci => ci.ProductVariant)
                                          .ToListAsync();

            if (!cartItems.Any())
            {
                _logger.LogWarning("Attempt to create order from empty cart for user {UserId}.", userId);
                return null;
            }

            // Verificar stock antes de crear el pedido
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
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    TotalPrice = 0,
                    OrderStatus = "Pending"
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync(); // Guarda el pedido para obtener su ID

                decimal totalOrderPrice = 0;
                foreach (var item in cartItems)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
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

                order.TotalPrice = totalOrderPrice;
                _context.Orders.Update(order);

                // Eliminar ítems del carrito después de crear el pedido
                _context.CartItems.RemoveRange(cartItems);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return order;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating order for user {UserId}", userId);
                throw;
            }
        }
    }
}