using Microsoft.EntityFrameworkCore;
using StockManager.API.Data;
using StockManager.API.Entities.DTOs.CatalogDTOs;
using StockManager.API.Entities.Models.Catalog;
using StockManager.API.Interfaces.CatalogInterfaces;
using StockManager.API.Middlewares.DomainExceptions;

namespace StockManager.API.Services.CatalogServices
{
    public class ProductService(DataBaseContext context) : IProductService
    {
        private readonly DataBaseContext _context = context;

        public async Task<CreatedProductDto> CreateProductAsync(CreateProductDto dto, string? urlPhoto)
        {
            await ValidateProductToCreateAsync(dto);
            
            var categories = await _context.Categories
            .Where(c => dto.CategoriesIds.Contains(c.Id))
            .ToListAsync();
            
            if (categories.Count != dto.CategoriesIds.Count()) throw new BusinessException("Una o más categorías no existen");

            var product = new Product
            {
                ProviderCode = dto.ProviderCode,
                ProductCode = dto.ProductCode,
                Categories = categories,
                Name = dto.Name,
                Price = dto.Price,   
                Stock = dto.Stock,
                UrlPhoto = urlPhoto
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return new CreatedProductDto(
                product.Id,
                product.ProviderCode,
                product.ProductCode,
                [.. categories.Select(c => new GetyCategoryNameDto(c.Name))],
                product.Name,
                product.Price,
                product.Stock,
                product.UrlPhoto
            );
        }

        public async Task<IEnumerable<GetOnlyProductDto>> GetAllProductsAsync()
        {
            return await _context.Products
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Select(p => new GetOnlyProductDto(
                    p.Id,
                    p.ProviderCode,
                    p.ProductCode,
                    p.Name,
                    p.Price)).ToListAsync();           
                    
        }

        public async Task<GetOnlyProductDto?> GetProductByCodesAsync(string providerCode, string productCode)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.ProviderCode == providerCode && p.ProductCode == productCode)
                .Select(p => new GetOnlyProductDto(
                    p.Id,
                    p.ProviderCode,
                    p.ProductCode,
                    p.Name,
                    p.Price))
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Producto no encontrado");
        }

        public async Task<GetOnlyProductDto?> GetProductById(int id)
        {
            return await _context.Products
               .IgnoreQueryFilters()
               .AsNoTracking()
               .Where(p => p.Id == id)
               .Select(p => new GetOnlyProductDto(
                   p.Id,
                    p.ProviderCode,
                    p.ProductCode,
                    p.Name,
                    p.Price))
               .FirstOrDefaultAsync() ?? throw new NotFoundException("ID de Producto no encontrado");
        }

        public async Task ToggleStatusProductAsync(int id)
        {
            var product = await _context.Products.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == id) ?? throw new NotFoundException($"Product not found");

            product.StatusActived = !product.StatusActived;
            await _context.SaveChangesAsync();
        }

        public async Task<GetOnlyProductDto> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            await ValidateProductToUpdateAsync(dto);

            var existing = await _context.Products
                .Include(p => p.Categories)
                .Include(p => p.Provider)
                .FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new NotFoundException("Producto no encontrado");

            existing.Name = dto.Name;
            existing.Price = dto.Price;
            existing.Stock = dto.Stock;
            existing.UrlPhoto = dto.UrlPhoto;

            await _context.SaveChangesAsync();
            await _context.Entry(existing)
            .Reference(p => p.Categories)
            .LoadAsync();

            return new GetOnlyProductDto(
                existing.Id,
                existing.Provider.Code,
                existing.ProductCode,
                existing.Name,
                existing.Price
            );
        }




        private async Task ValidateProductToCreateAsync(CreateProductDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ProviderCode))
                throw new BusinessException("Se necesita un código de proveedor");

            if (string.IsNullOrWhiteSpace(dto.ProductCode))
                throw new BusinessException("Se necesita un código de proveedor para el producto");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new BusinessException("Se necesita el nombre del producto");

            if (dto.Price <= 0)
                throw new BusinessException("El stock no puede ser negativo al crearse el producto");


            var providerExists = await _context.Providers.AnyAsync(p => p.Code == dto.ProviderCode);
            if (!providerExists)
                throw new InvalidOperationException($"No existe ningún proveedor con el código {dto.ProviderCode}");


            /*var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == dto.CategoryId);

            if (!categoryExists)
                throw new InvalidOperationException("La categoría no existe o está desactivada");*/


            var codeInUse = await _context.Products.AnyAsync(p =>
                p.ProductCode == dto.ProductCode &&
                p.ProviderCode== dto.ProviderCode);

            if (codeInUse)
                throw new BusinessException(
                    $"El código '{dto.ProductCode}' ya está asignado a otro producto de este proveedor.");
        }

        private static Task ValidateProductToUpdateAsync(UpdateProductDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new BusinessException("El producto debe tener nombre.");

            if (dto.Price <= 0)
                throw new BusinessException("El precio no puede ser negativo o cero");
            if (dto.Stock < 0)
                throw new BusinessException("El stock debe ser mayor o igual que cero");

            return Task.CompletedTask;

        }
    }
}

