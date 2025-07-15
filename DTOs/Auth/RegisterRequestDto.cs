using System.ComponentModel.DataAnnotations;

namespace SolYSalEcommerce.DTOs.Auth
{
    public class RegisterRequestDto
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(6)] // Contraseña debe tener al menos 6 caracteres
        public string Password { get; set; } = string.Empty;
    }
}
