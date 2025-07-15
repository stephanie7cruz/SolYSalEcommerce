using SolYSalEcommerce.DTOs.Cart; 
using SolYSalEcommerce.DTOs.Products; 

namespace SolYSalEcommerce.DTOs.Orders
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        // Agregamos OrderNumber y CreatedAt
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty; // Estado de la orden

        public decimal TotalAmount { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
    }

    public class OrderDetailDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductVariantId { get; set; } // Referencia a la variante del producto
        public ProductDto Product { get; set; } = new ProductDto(); // Datos del producto, si es necesario
        public ProductVariantDto ProductVariant { get; set; } = new ProductVariantDto(); // Datos de la variante
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
