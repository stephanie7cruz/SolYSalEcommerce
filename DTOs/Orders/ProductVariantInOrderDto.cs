using System;
using System.Collections.Generic; 

namespace SolYSalEcommerce.DTOs.Orders
{
    // Este DTO solo contiene las propiedades relevantes de la variante para un item de orden
    // Sin referencias a su Product si Product.Variants crea un ciclo
    public class ProductVariantInOrderDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; } 
        public string Sku { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal BasePrice { get; set; } 
        public string ProductName { get; set; } = string.Empty; 
    }

}