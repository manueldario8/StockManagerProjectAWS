using Microsoft.AspNetCore.Mvc;
using StockManager.API.Entities.DTOs.UserDTOs;
using StockManager.API.Interfaces.AuthInterfaces;

namespace StockManager.API.Controllers.AuthControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "Conexión correcta con User" });
        }

        [HttpPost]
        public async Task<ActionResult<CreatedUserDTO>> Create(CreateUserDTO dto)
        {
            return Ok(await _userService.CreateUser(dto));
        }
    }
}
