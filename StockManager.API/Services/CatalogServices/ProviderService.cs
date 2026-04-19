using StockManager.API.Data;
using StockManager.API.Entities.DTOs.CatalogDTOs;
using StockManager.API.Entities.Models.Catalog;
using StockManager.API.Interfaces.CatalogInterfaces;
using Microsoft.EntityFrameworkCore;
using StockManager.API.Middlewares.DomainExceptions;

namespace StockManager.API.Services.CatalogServices
{
    public class ProviderService(DataBaseContext context) : IProviderService
    {
        private readonly DataBaseContext _context = context;

        public async Task<CreatedProviderDto> CreateProviderAsync(CreateProviderDto dto)
        {
            await ValidateProviderAsync(dto.Name, dto.Code);

            var provider = new Provider
            {
                Name = dto.Name,
                Code = dto.Code
            };

            await _context.Providers.AddAsync(provider);
            await _context.SaveChangesAsync();

            return new CreatedProviderDto(provider.Id, provider.Name, provider.Code);
        }

        public async Task<IEnumerable<GetOnlyProviderDto>> GetAllProvidersAsync()
        {
            return await _context.Providers
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Select(p => new GetOnlyProviderDto(p.Id, p.Name, p.Code, p.StatusActived))
                .ToListAsync();
        }

        public async Task<GetProviderWithProductsDto?> GetProviderByIdAsync(int id)
        {
            return await _context.Providers
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Where(p => p.Id == id)
                .Select(p => new GetProviderWithProductsDto(
                    p.Name,
                    p.Code,
                    p.Products.Select(d => new GetProductToProviderDto(
                        d.ProviderCode,
                        d.ProductCode,
                        d.Name,
                        d.Price,
                        d.Stock))))
                .FirstOrDefaultAsync() ?? throw new NotFoundException("No se encontró ningún proveedor con ese ID");
        }

        public async Task<GetStockByProviderDto?> GetStockByProviderAsync(int id)
        {
            return await _context.Providers
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Where(p => p.Id == id)
                .Select(p => new GetStockByProviderDto(
                    p.Name,
                    p.Code,
                    p.Products.Select(d => new GetProductToStockDto(
                        d.ProviderCode,
                        d.ProductCode,
                        d.Name,
                        d.Stock))))
                .FirstOrDefaultAsync() ?? throw new NotFoundException("No se encontró ningún proveedor con ese ID");
        }

        public async Task ToggleStatusProviderAsync(int id)
        {
            var existing = await _context.Providers.FindAsync(id) ?? throw new NotFoundException("No se encontró el proveedor");
            existing.StatusActived = !existing.StatusActived;

            await _context.SaveChangesAsync();
        }

        public async Task<GetOnlyProviderDto> UpdateProviderAsync(int id, UpdateProviderDto dto)
        {
            var existing = await _context.Providers.FindAsync(id) ?? throw new NotFoundException("No se encontró el proveedor");

            await ValidateProviderAsync(dto.Name, existing.Code, id);
            existing.Name = dto.Name;

            await _context.SaveChangesAsync();
            return new GetOnlyProviderDto(existing.Id, existing.Code, dto.Name, existing.StatusActived);
        }




        private async Task ValidateProviderAsync(string name, string code, int? id = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new InvalidOperationException("El nombre no puede estar vacío");
            if (string.IsNullOrWhiteSpace(code)) throw new InvalidOperationException("El código no puede estar vacío");

            var codeInUse = await _context.Providers.AnyAsync(p => p.Code.ToLower() == code.ToLower() && (!id.HasValue || p.Id != id.Value));


            if (codeInUse) throw new InvalidOperationException( $"El código '{code}' ya está siendo usado por otro proveedor");
        }



    }
}
