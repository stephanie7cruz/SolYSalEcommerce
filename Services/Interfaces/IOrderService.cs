using SolYSalEcommerce.DTOs.Orders; 
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolYSalEcommerce.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetMyOrdersAsync(Guid userId);
        Task<OrderDto?> GetOrderByIdAsync(Guid orderId, Guid userId);
        Task<CreateOrderResponseDto?> CreateOrderFromCartAsync(Guid userId);
    }
}