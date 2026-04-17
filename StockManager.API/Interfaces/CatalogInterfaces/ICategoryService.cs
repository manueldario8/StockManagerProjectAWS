using StockManager.API.Entities.DTOs.CatalogDTOs;

namespace StockManager.API.Interfaces.CatalogInterfaces
{
    public interface ICategoryService
    {
        Task<CreatedCategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
        Task<GetOnlyCategoryDto> UpdateCategoryAsync(int id, UpdateCategoryDto dto);
        Task<IEnumerable<GetOnlyCategoryDto>> GetAllCategoriesAsync();
        Task<GetCategoryWithProductsDto?> GetCategoryByIdAsync(int id);
        Task ToggleCategoryByAdminAsync(int id);
    }
}
