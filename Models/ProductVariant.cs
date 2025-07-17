using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolYSalEcommerce.Models
{
    public class ProductVariant
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!; // Propiedad de navegación

        [Required]
        [MaxLength(100)]
        public string Color { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Size { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string SKU { get; set; } = string.Empty;

        [Required]
        public int Stock { get; set; } // <--- Esta propiedad debe existir

        [Required]
        [Column(TypeName = "decimal(18,2)")] // Precio de la variante (puede ser diferente del BasePrice del producto)
        public decimal BasePrice { get; set; } // <--- Esta propiedad debe existir (la habías usado en OrderService)

        [MaxLength(500)]
        public string? ImageUrl { get; set; } // URL de la imagen principal de la variante

        // Propiedad de navegación inversa
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}