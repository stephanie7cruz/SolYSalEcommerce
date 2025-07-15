using System.ComponentModel.DataAnnotations;

namespace SolYSalEcommerce.DTOs.Cart
{
    public class UpdateCartItemDto
    {
        [Required]
        public Guid ProductVariantId { get; set; } // Identificador de la variante a actualizar
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad debe ser 0 o más.")] // 0 para eliminar
        public int Quantity { get; set; }
    }
}