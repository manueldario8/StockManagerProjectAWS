using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockManager.API.Entities.DTOs.CatalogDTOs;
using StockManager.API.Interfaces.CatalogInterfaces;

namespace StockManager.API.Controllers.CatalogControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {
        private readonly ICategoryService _categoryService = categoryService;

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            var created = await _categoryService.CreateCategoryAsync(dto);
            return CreatedAtAction(nameof(GetCategoryByIdAdmin), new { id = created.Id }, created);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesByAdmins()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoryByIdAdmin(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(category);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto dto)
        {
            var updated = await _categoryService.UpdateCategoryAsync(id, dto);
            return Ok(updated);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("toggle/{id:int}")]
        public async Task<IActionResult> ChangeCategoryStatus(int id)
        {
            await _categoryService.ToggleCategoryByAdminAsync(id);
            return NoContent();
        }

       
    }
}
