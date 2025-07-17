using System.ComponentModel.DataAnnotations;
using System.Collections.Generic; 

namespace SolYSalEcommerce.DTOs.Products
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "El nombre del producto es requerido.")]
        [MaxLength(200, ErrorMessage = "El nombre no puede exceder los 200 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000, ErrorMessage = "La descripción no puede exceder los 1000 caracteres.")]
        public string? Description { get; set; } // Puede ser nulo

        [Required(ErrorMessage = "El precio base del producto es requerido.")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "El precio base debe ser mayor a cero.")] // Uso de decimal.MaxValue
        public decimal BasePrice { get; set; }

        public bool IsActive { get; set; } = true; // Por defecto, el producto está activo

        // Una lista de variantes que se crearán junto con el producto.
        // Cada producto de Sol y Sal puede tener varias combinaciones de color y talla.
        // Debe ser inicializada para evitar NullReferenceException
        public List<CreateProductVariantDto> Variants { get; set; } = new List<CreateProductVariantDto>();
    }

    // Clase anidada para las variantes del producto al momento de la creación
    public class CreateProductVariantDto
    {
        [Required(ErrorMessage = "El color de la variante es requerido.")]
        [MaxLength(50, ErrorMessage = "El color no puede exceder los 50 caracteres.")]
        public string Color { get; set; } = string.Empty;

        [Required(ErrorMessage = "La talla de la variante es requerida.")]
        [MaxLength(20, ErrorMessage = "La talla no puede exceder los 20 caracteres.")]
        public string Size { get; set; } = string.Empty;

        [Required(ErrorMessage = "El SKU de la variante es requerido.")]
        [MaxLength(100, ErrorMessage = "El SKU no puede exceder los 100 caracteres.")]
        public string SKU { get; set; } = string.Empty; // Código único de inventario para esta variante

        [Required(ErrorMessage = "El stock de la variante es requerido.")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; } // Cantidad disponible

        [MaxLength(500, ErrorMessage = "La URL de la imagen no puede exceder los 500 caracteres.")]
        public string? ImageUrl { get; set; } // URL de la imagen específica para esta variante

        // Permite especificar un precio base diferente para la variante.
        // Si es nulo, el servicio debería tomar el BasePrice del producto padre.
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "El precio base de la variante debe ser mayor a cero.")]
        public decimal? BasePrice { get; set; } // Hacemos la propiedad anulable (nullable)
    }
}