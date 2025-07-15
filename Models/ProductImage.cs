using SolYSalEcommerce.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolYSalEcommerce.Data.Models
{
    public class ProductImage
    {
        public Guid Id { get; set; } // Identificador único para la imagen
        public string ImageUrl { get; set; } // URL de la imagen
        public int Order { get; set; } // Orden de visualización de la imagen (ej: 1, 2, 3)

        // Clave foránea para el Product al que pertenece esta imagen
        // Una imagen siempre pertenecerá a un producto.
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        // Clave foránea para el ProductVariant al que pertenece esta imagen.
        // Es anulable (Guid?) porque no todas las imágenes tienen que ser específicas de una variante de color/talla.
        // Pero en tu caso, para las imágenes por color, SÍ se asociarán a una variante.
        public Guid? ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; }
    }
}