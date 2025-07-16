using System; // Para Guid
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SolYSalEcommerce.DTOs.Products
{
    public class UpdateProductDto
    {
        [Required(ErrorMessage = "El nombre del producto es requerido.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 200 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000, ErrorMessage = "La descripción no puede exceder los 1000 caracteres.")]
        public string? Description { get; set; } // Puede ser nulo o vacío

        [Required(ErrorMessage = "El precio base del producto es requerido.")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "El precio base debe ser mayor a cero.")]
        public decimal BasePrice { get; set; }

        public bool IsActive { get; set; } = true; // Por defecto, el producto está activo

        // La lista de variantes para actualizar.
        // Debe ser inicializada para evitar NullReferenceException
        public List<UpdateProductVariantDto> Variants { get; set; } = new List<UpdateProductVariantDto>();
    }

    public class UpdateProductVariantDto
    {
        // ESTO ES LO CLAVE: El ID de la variante.
        // Debe ser Guid? (nullable) porque si es una variante nueva que se añade en la actualización, no tendrá un ID.
        // Si es una variante existente, el cliente DEBE enviarlo.
        public Guid? Id { get; set; } // Hacemos el ID nullable para nuevas variantes

        [Required(ErrorMessage = "El color de la variante es requerido.")]
        [MaxLength(50, ErrorMessage = "El color no puede exceder los 50 caracteres.")]
        public string Color { get; set; } = string.Empty;

        [Required(ErrorMessage = "La talla de la variante es requerida.")]
        [MaxLength(20, ErrorMessage = "La talla no puede exceder los 20 caracteres.")]
        public string Size { get; set; } = string.Empty;

        [Required(ErrorMessage = "El SKU de la variante es requerido.")]
        [MaxLength(100, ErrorMessage = "El SKU no puede exceder los 100 caracteres.")]
        public string SKU { get; set; } = string.Empty; // Stock Keeping Unit

        [Required(ErrorMessage = "El stock de la variante es requerido.")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; } // Cantidad disponible

        [MaxLength(500, ErrorMessage = "La URL de la imagen no puede exceder los 500 caracteres.")]
        public string? ImageUrl { get; set; } // URL de la imagen específica para esta variante

        // **NUEVO:** Permite especificar un precio base diferente para la variante en la actualización.
        // Si es nulo, el servicio debería mantener el precio existente o re-heredar del padre.
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "El precio base de la variante debe ser mayor a cero.")]
        public decimal? BasePrice { get; set; } // Hacemos la propiedad anulable (nullable)
    }
}