using System;
using System.ComponentModel.DataAnnotations.Schema; 

namespace SolYSalEcommerce.Models
{
    public class CartItem 
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public DateTime DateAdded { get; set; }

        // Propiedades de navegación
        public User User { get; set; } = default!; // Para evitar advertencias de nullability
        public ProductVariant ProductVariant { get; set; } = default!; // Para evitar advertencias de nullability
    }
}