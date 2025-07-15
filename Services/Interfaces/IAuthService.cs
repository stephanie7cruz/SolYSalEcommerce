using SolYSalEcommerce.DTOs.Auth;
using SolYSalEcommerce.Models; 

namespace SolYSalEcommerce.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User?> Register(RegisterRequestDto request); // Retornar el modelo User es aceptable para fines internos
        Task<string?> Login(LoginRequestDto request); // Devuelve el token JWT
    }
}
