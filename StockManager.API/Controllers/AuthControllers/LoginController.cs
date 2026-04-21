using Microsoft.AspNetCore.Mvc;
using StockManager.API.Entities.DTOs.UserDTOs;
using StockManager.API.Interfaces.AuthInterfaces;

namespace StockManager.API.Controllers.AuthControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "Conexión correcta con Login" });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO dto)
        {
            return Ok(await _authService.LoginAsync(dto));
        }
    }
}
