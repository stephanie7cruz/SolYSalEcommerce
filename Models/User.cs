using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SolYSalEcommerce.Models
{
    // Clase User hereda de IdentityUser<Guid>
    // Esto la integra con el sistema de usuarios de ASP.NET Core Identity
    public class User : IdentityUser<Guid>
    {
        // Las propiedades como Id, Email, PasswordHash (y UserName) ya son proporcionadas por IdentityUser<Guid>.

        // Propiedades personalizadas que no están en IdentityUser:
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        // Propiedades de navegación para CarritoItem
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        // Propiedad de navegación para Pedidos
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}