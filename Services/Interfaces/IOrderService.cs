// Services/Interfaces/IOrderService.cs
using SolYSalEcommerce.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolYSalEcommerce.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetMyOrdersAsync(Guid userId);
        Task<Order?> GetOrderByIdAsync(Guid orderId, Guid userId);
        Task<Order?> CreateOrderFromCartAsync(Guid userId);
        // Métodos relacionados con pagos NO estarían aquí todavía
    }
}