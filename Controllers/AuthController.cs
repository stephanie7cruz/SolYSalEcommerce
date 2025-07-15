using Microsoft.AspNetCore.Mvc;
using SolYSalEcommerce.DTOs.Auth;
using SolYSalEcommerce.Services.Interfaces; 

namespace SolYSalEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService; 

        public AuthController(IAuthService authService) 
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _authService.Register(request);
            if (user == null)
            {
                return BadRequest(new { Message = "El email ya está registrado." });
            }

            return StatusCode(201, new { Message = "Registro exitoso", UserId = user.Id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _authService.Login(request);
            if (token == null)
            {
                return Unauthorized(new { Message = "Credenciales inválidas." });
            }

            return Ok(new { Token = token });
        }
    }
}