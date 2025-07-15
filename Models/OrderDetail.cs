using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolYSalEcommerce.Models
{
    public class OrderDetail
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!; // Propiedad de navegación

        [Required]
        public Guid ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; } = null!; // Propiedad de navegación

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerUnit { get; set; } // <--- Esta propiedad debe existir
    }
}