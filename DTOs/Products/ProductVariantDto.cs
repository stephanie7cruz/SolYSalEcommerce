using System; 

namespace SolYSalEcommerce.DTOs.Products
{
    public class ProductVariantDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; } 
        public string Color { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public int Stock { get; set; }
        public string? ImageUrl { get; set; } // Puede ser nulo
        public decimal BasePrice { get; set; }
    }
}