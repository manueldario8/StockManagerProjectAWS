using Microsoft.AspNetCore.Mvc;
using StockManager.API.Entities.DTOs.CatalogDTOs;
using StockManager.API.Interfaces.CatalogInterfaces;

namespace StockManager.API.Controllers.CatalogControllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
    public class ProviderController(IProviderService providerService) : ControllerBase
    {
        private readonly IProviderService _providerService = providerService;

        
        [HttpPost]
        public async Task<IActionResult> CreateProvider([FromBody] CreateProviderDto dto)
        {
            var created = await _providerService.CreateProviderAsync(dto);
            return CreatedAtAction(nameof(GetProviderById), new { id = created.Id }, created);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllProviders()
        {
            var providers = await _providerService.GetAllProvidersAsync();
            return Ok(providers);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProviderById(int id)
        {
            var provider = await _providerService.GetProviderByIdAsync(id);
            return Ok(provider);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProvider(int id, [FromBody] UpdateProviderDto dto)
        {
            var updated = await _providerService.UpdateProviderAsync(id, dto);
            return Ok(updated);
        }

        [HttpPatch("toggle/{id:int}")]
        public async Task<IActionResult> ToggleStatusActived(int id)
        {
            await _providerService.ToggleStatusProviderAsync(id);
            return NoContent();
        }

        [HttpGet("stock/{id:int}")]
        public async Task<IActionResult> GetStockByProviderById(int id)
        {
            var provider = await _providerService.GetStockByProviderAsync(id);
            return Ok(provider);
        }
    }
}
