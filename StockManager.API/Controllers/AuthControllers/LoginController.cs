using Microsoft.AspNetCore.Mvc;
using StockManager.API.Entities.DTOs.UserDTOs;
using StockManager.API.Interfaces.AuthInterfaces;

namespace StockManager.API.Controllers.AuthControllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;


        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO dto)
        {
            return Ok(await _authService.LoginAsync(dto));
        }
    }
}
