using System;
using System.Collections.Generic;

namespace SolYSalEcommerce.DTOs.Orders
{
    public class OrderDto // Para las respuestas GET de órdenes
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string OrderNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty; // Estado de la orden

        public decimal TotalAmount { get; set; }

        // Aquí se usa la versión segura de OrderDetailDto
        public List<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();

    }
}