using StockManager.API.Entities.DTOs.CatalogDTOs;

namespace StockManager.API.Interfaces.CatalogInterfaces
{
    public interface IProductService
    {
        Task<CreatedProductDto> CreateProductAsync(CreateProductDto dto, string? urlPhoto);
        Task<GetOnlyProductDto> UpdateProductAsync(int id, UpdateProductDto dto);
        Task<IEnumerable<GetOnlyProductDto>> GetAllProductsAsync();
        Task<GetOnlyProductDto?> GetProductByCodesAsync(string providerCode, string productCode);
        Task<GetOnlyProductDto?> GetProductById(int id);
        Task ToggleStatusProductAsync(int id);
    }
}
