using System;
using System.Collections.Generic; // Si tu ProductInOrderDto tiene variantes

namespace SolYSalEcommerce.DTOs.Orders
{
    // Este DTO solo contiene las propiedades relevantes de la variante para un item de orden
    // Sin referencias a su Product si Product.Variants crea un ciclo
    public class ProductVariantInOrderDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; } // Puedes mantener el ID del producto padre si es útil
        public string Sku { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal BasePrice { get; set; } // El precio base actual de la variante (no el de la orden)

        // Si necesitas el nombre del producto, lo puedes poner aquí directamente,
        // o crear un ProductInOrderDto sin la colección de variantes.
        public string ProductName { get; set; } = string.Empty; // <-- Añadido para aplanar
    }

    // Si tu ProductDto tiene una lista de ProductVariantDto, entonces ProductDto también necesita
    // ser aplanado para evitar el ciclo cuando se usa dentro de OrderDetailDto
    // public class ProductInOrderDto
    // {
    //     public Guid Id { get; set; }
    //     public string Name { get; set; } = string.Empty;
    //     public string Description { get; set; } = string.Empty;
    //     // NO INCLUIR List<ProductVariantDto> Variants aquí si esta es la fuente del ciclo
    // }
}