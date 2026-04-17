using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManager.API.Entities.DTOs.CatalogDTOs;
using StockManager.API.Interfaces.CatalogInterfaces;

namespace StockManager.API.Controllers.CatalogControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService productService, IImageService imageService) : ControllerBase
    {
        private readonly IProductService _productService = productService;
        private readonly IImageService _imageService = imageService;

        /*Endpoints to be used by admins*/
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto dto)
        {
            string? urlPhoto = null;

            if (dto.Image is not null)
            {
                using var stream = dto.Image.OpenReadStream();
                urlPhoto = await _imageService.UploadImage(
                    stream,
                    dto.Image.FileName
                );
            }

            var created = await _productService.CreateProductAsync(dto, urlPhoto);
            return CreatedAtAction(nameof(GetProductById), new { id = created.Id }, created);

        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllProductsByAdmins()
        {
            return Ok(await _productService.GetAllProductsAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("adm/{id:int}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductById(id);
            return Ok(product);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("{providerCode}/{productCode}")]
        public async Task<IActionResult> GetProductByCodesAdmin(string providerCode, string productCode)
        {
            var product = await _productService.GetProductByCodesAsync(providerCode, productCode);
            return Ok(product);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto dto)
        {
            var updated = await _productService.UpdateProductAsync(id, dto);
            return Ok(updated);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("toggle/{id:int}")]
        public async Task<IActionResult> ToggleProductStatus(int id)
        {

            await _productService.ToggleStatusProductAsync(id);
            return NoContent();

        }
    }
}
