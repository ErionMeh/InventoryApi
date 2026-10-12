using InventoryApi.Data;
using InventoryApi.Dtos;
using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;


namespace InventoryApi.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<bool> SkuExistsAsync(string sku);
        Task<ProductDto> CreateAsync(ProductCreateDto dto);
        Task<bool> UpdateAsync(int id, ProductUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }

    public class ProductService : IProductService
    {
        private readonly AppDbContext _db;

        public ProductService(AppDbContext db) => _db = db;

        public Task<List<ProductDto>> GetAllAsync() =>
            _db.Products
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .Select(p => new ProductDto(p.Id, p.Name, p.Sku, p.Price, p.QuantityInStock))
                .ToListAsync();

        public Task<ProductDto?> GetByIdAsync(int id) =>
            _db.Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductDto(p.Id, p.Name, p.Sku, p.Price, p.QuantityInStock))
                .FirstOrDefaultAsync();

        public Task<bool> SkuExistsAsync(string sku) =>
            _db.Products.AnyAsync(p => p.Sku == sku);

        public async Task<ProductDto> CreateAsync(ProductCreateDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Sku = dto.Sku,
                Price = dto.Price,
                QuantityInStock = dto.QuantityInStock
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return new ProductDto(product.Id, product.Name, product.Sku, product.Price, product.QuantityInStock);
        }

        public async Task<bool> UpdateAsync(int id, ProductUpdateDto dto)
        {
            var product = await _db.Products.FindAsync(id);
            if (product is null) return false;

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.QuantityInStock = dto.QuantityInStock;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product is null) return false;

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
