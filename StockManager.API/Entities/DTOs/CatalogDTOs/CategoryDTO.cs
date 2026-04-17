namespace StockManager.API.Entities.DTOs.CatalogDTOs
{
    public record CreateCategoryDto(
        string Name);

    public record CreatedCategoryDto(
        int Id,
        string Name);

    public record UpdateCategoryDto(
        string Name);

    public record GetyCategoryNameDto(
        string Name);

    public record GetOnlyCategoryDto(
        int Id,
        string Name,
        bool StatusActived);

    public record GetCategoryWithProductsDto(
        string Name,
        IEnumerable<GetProductToCategoryDto> Products);
}
