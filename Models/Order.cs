// Models/Order.cs

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // ¡Asegúrate de que este using esté aquí!

namespace SolYSalEcommerce.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!; // Propiedad de navegación

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        // ¡¡¡AÑADE ESTA LÍNEA!!!
        [Column(TypeName = "decimal(18,2)")] // Define precisión: 18 dígitos en total, 2 después del punto decimal
        public decimal TotalPrice { get; set; }

        [Required]
        [MaxLength(50)]
        public string OrderStatus { get; set; } = string.Empty;

        // Propiedad de navegación para los detalles del pedido
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}