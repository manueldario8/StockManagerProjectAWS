using Microsoft.AspNetCore.Mvc;
using StockManager.API.Entities.DTOs.UserDTOs;

namespace StockManager.API.Controllers.CatalogControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PruebaController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "Conexión correcta con Login" });
        }

        
    }
}
