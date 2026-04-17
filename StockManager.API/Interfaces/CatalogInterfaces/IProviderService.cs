using StockManager.API.Entities.DTOs.CatalogDTOs;

namespace StockManager.API.Interfaces.CatalogInterfaces
{
    public interface IProviderService
    {
        Task<CreatedProviderDto> CreateProviderAsync(CreateProviderDto dto);
        Task<GetOnlyProviderDto> UpdateProviderAsync(int id, UpdateProviderDto dto);
        Task<IEnumerable<GetOnlyProviderDto>> GetAllProvidersAsync();
        Task<GetProviderWithProductsDto?> GetProviderByIdAsync(int id);
        Task ToggleStatusProviderAsync(int id);
        Task<GetStockByProviderDto?> GetStockByProviderAsync(int id);
    }
}
