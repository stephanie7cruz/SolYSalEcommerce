using SolYSalEcommerce.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolYSalEcommerce.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")] // Para precisión de moneda
        public decimal BasePrice { get; set; } 
        public bool IsActive { get; set; } = true; 

        // Propiedad de navegación para Variantes de Producto
        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>(); 

        // Propiedad de navegación para las imágenes asociadas a este producto
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    }
}