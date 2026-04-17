using Microsoft.AspNetCore.Http;

namespace StockManager.API.Entities.DTOs.CatalogDTOs
{
   
    public record CreateProductDto(
       string ProviderCode,
       string ProductCode,
       IEnumerable<int> CategoriesIds,
       string Name,
       decimal Price,
       int Stock,
       IFormFile? Image);

    public record CreatedProductDto(
        int Id,
        string ProviderCode,
        string ProductCode,
        IEnumerable<GetyCategoryNameDto> Categories,
        string Name,
        decimal Price,
        int Stock,
        string? UrlPhoto);

    public record UpdateProductDto(
        string Name,
        IEnumerable<GetyCategoryNameDto> Categories,
        decimal Price,
        int Stock,
        string? UrlPhoto);

    public record GetOnlyProductDto(
        int Id,
        string ProviderCode,
        string ProductCode,
        string Name,
        decimal Price);

    public record GetProductToCategoryDto(
        string ProviderCode,
        string ProductCode,
        string Name,
        decimal Price,
        IEnumerable<GetyCategoryNameDto> Categories);

    public record GetProductToProviderDto(
        string ProviderCode,
        string ProductCode,
        string Name,
        decimal Price,
        int Stock);

    public record GetProductToStockDto(
        string ProviderCode,
        string ProductCode,
        string Name,
        int Stock);
}
