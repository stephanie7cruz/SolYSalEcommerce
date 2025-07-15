using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity; // ¡¡¡IMPORTANTE: Necesitas este using para IdentityUser!!!

namespace SolYSalEcommerce.Models
{
    // Tu clase User ahora hereda de IdentityUser<Guid>
    // Esto la integra con el sistema de usuarios de ASP.NET Core Identity
    public class User : IdentityUser<Guid>
    {
        // Las propiedades como Id, Email, PasswordHash (y UserName) ya son proporcionadas por IdentityUser<Guid>.
        // NO las declares de nuevo aquí, a menos que quieras sobreescribir su comportamiento (lo cual es raro y avanzado).

        // Aquí solo defines tus propiedades personalizadas que no están en IdentityUser:
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        // La propiedad 'Role' no se maneja directamente aquí cuando usas IdentityRole.
        // Se gestiona a través del sistema de roles de Identity (UserRoles, etc.).
        // Si necesitas un campo personalizado para un rol predeterminado o algo así,
        // esto sería un campo adicional y no el rol principal de Identity.
        // Por ahora, te recomiendo quitar 'public string Role { get; set; } = "Client";'
        // y usar el sistema de roles de ASP.NET Core Identity.

        // Propiedades de navegación para CarritoItem
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        // Propiedad de navegación para Pedidos
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}