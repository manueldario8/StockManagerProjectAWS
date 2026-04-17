using Microsoft.EntityFrameworkCore;
using StockManager.API.Data;
using StockManager.API.Entities.DTOs.CatalogDTOs;
using StockManager.API.Entities.Models.Catalog;
using StockManager.API.Interfaces.CatalogInterfaces;
using StockManager.API.Middlewares.DomainExceptions;

namespace StockManager.API.Services.CatalogServices
{
    public class CategoryService(DataBaseContext context) : ICategoryService
    {
        private readonly DataBaseContext _context = context;

        public async Task<CreatedCategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
        {
            await ValidateCategoryAsync(dto.Name);

            var category = new Category
            {
                Name = dto.Name
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return new CreatedCategoryDto(category.Id, category.Name);
        }

        public async Task<IEnumerable<GetOnlyCategoryDto>> GetAllCategoriesAsync()
        {
            return await _context.Categories
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Select(c => new GetOnlyCategoryDto(c.Id, c.Name, c.StatusActived))
            .ToListAsync();
        }

        public async Task<GetCategoryWithProductsDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories
               .AsNoTracking()
               .Where(c => c.Id == id)
               .Select(c => new GetCategoryWithProductsDto(
                   c.Name,
                   c.Products.Select(p => new GetProductToCategoryDto(
                       p.Provider.Code,
                       p.ProductCode,
                       p.Name,
                       p.Price,
                       p.Categories.Select(q => new GetyCategoryNameDto(
                           q.Name))
                   ))
               ))
               .IgnoreQueryFilters()
               .FirstOrDefaultAsync()??throw new NotFoundException("La categoría no existe");

            return category;
        }

        public async Task ToggleCategoryByAdminAsync(int id)
        {
            var existing = await _context.Categories.FindAsync(id) ?? throw new NotFoundException("No se encontró la categoría con ese ID");
            existing.StatusActived = !existing.StatusActived;

            await _context.SaveChangesAsync();
        }

        public async Task<GetOnlyCategoryDto> UpdateCategoryAsync(int id, UpdateCategoryDto dto)
        {
            var existing = await _context.Categories.FindAsync(id) ?? throw new NotFoundException("No se encontró una categoría con ese ID");

            await ValidateCategoryAsync(dto.Name, id);

            existing.Name = dto.Name;

            await _context.SaveChangesAsync();

            return new GetOnlyCategoryDto(existing.Id, existing.Name, existing.StatusActived);
        }



        private async Task ValidateCategoryAsync(string name, int? categoryId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new BusinessException("El nombre no puede estar vacío");

            var exists = await _context.Categories.AnyAsync(c =>
                c.Name.ToLower() == name.ToLower() &&
                (!categoryId.HasValue || c.Id != categoryId.Value));

            if (exists)
                throw new BusinessException(
                    $"La categoría '{name}' ya existe.");
        }
    }
}
