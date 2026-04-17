namespace StockManager.API.Entities.DTOs.CatalogDTOs
{
    public record CreateProviderDto(
         string Name,
         string Code);

    public record CreatedProviderDto(
        int Id,
        string Name,
        string Code);

    public record UpdateProviderDto(
        string Name);

    public record GetOnlyProviderDto(
        int Id,
        string Name,
        string Code,
        bool StatusActived);

    public record GetProviderWithProductsDto(
        string Name,
        string Code,
        IEnumerable<GetProductToProviderDto> Products);

    public record GetStockByProviderDto(
        IEnumerable<GetProductToStockDto> ProductsDto);

}
