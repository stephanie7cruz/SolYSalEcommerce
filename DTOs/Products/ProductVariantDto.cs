using System; // Para Guid

namespace SolYSalEcommerce.DTOs.Products
{
    public class ProductVariantDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; } // Añadir si necesitas el ID del producto padre en la variante DTO
        public string Color { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public int Stock { get; set; }
        public string? ImageUrl { get; set; } // Puede ser nulo

        // ¡NUEVO! Añadir la propiedad BasePrice para que pueda ser mapeada y devuelta
        public decimal BasePrice { get; set; }
    }
}