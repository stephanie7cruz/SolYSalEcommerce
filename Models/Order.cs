

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolYSalEcommerce.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!; // Propiedad de navegación

        // número de orden único
        [MaxLength(50)]
        public string OrderNumber { get; set; } = string.Empty; 

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; } 

        [Required]
        [MaxLength(50)]
        public string OrderStatus { get; set; } = string.Empty; 

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}