using InventoryApi.Data;
using InventoryApi.Dtos;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Services
{
    public interface IReportService
    {
        Task<List<ProductDto>> GetLowStockAsync(int threshold);
        Task<List<TopProductDto>> GetTopOrderedAsync(int count);
    }

    public class ReportService : IReportService
    {
        private readonly AppDbContext _db;

        public ReportService(AppDbContext db) => _db = db;

        public async Task<List<ProductDto>> GetLowStockAsync(int threshold)
        {
            return await _db.Products
                .AsNoTracking()
                .Where(p => p.QuantityInStock < threshold)
                .OrderBy(p => p.QuantityInStock)
                .Select(p => new ProductDto(p.Id, p.Name, p.Sku, p.Price, p.QuantityInStock))
                .ToListAsync();
        }

        public async Task<List<TopProductDto>> GetTopOrderedAsync(int count)
        {
            return await _db.OrderItems
                .AsNoTracking()
                .GroupBy(i => new { i.ProductId, i.Product.Name })
                .OrderByDescending(items => items.Sum(i => i.Quantity)) 
                .Take(count) 
                .Select(g => new TopProductDto(g.Key.ProductId, g.Key.Name, g.Sum(i => i.Quantity)))
                .ToListAsync();
        }
    }
}
