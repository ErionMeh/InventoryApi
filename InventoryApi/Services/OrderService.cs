using InventoryApi.Data;
using InventoryApi.Dtos;
using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;


namespace InventoryApi.Services
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllAsync();
        Task<OrderDto?> GetByIdAsync(int id);
        Task<OrderDto> CreateAsync(OrderCreateDto dto);
    }

    public class OrderService : IOrderService
    {
        private readonly AppDbContext _db;

        public OrderService(AppDbContext db) => _db = db;

        public async Task<List<OrderDto>> GetAllAsync()
        {
            var orders = await _db.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return orders.Select(ToDto).ToList();
        }

        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await _db.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order is null ? null : ToDto(order);
        }

        public async Task<OrderDto> CreateAsync(OrderCreateDto dto)
        {
            var ids = dto.Items.Select(i => i.ProductId).ToList();


            var products = await _db.Products
                .Where(p => ids.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            var order = new Order { CreatedAt = DateTime.UtcNow };

            foreach (var item in dto.Items)
            {
                if (!products.ContainsKey(item.ProductId))
                {
                    throw new ProductNotFoundException(item.ProductId);
                }

                var product = products[item.ProductId];


                if (product.QuantityInStock < item.Quantity)
                {
                    throw new InsufficientStockException(product.Name, item.Quantity, product.QuantityInStock);
                }
                product.QuantityInStock -= item.Quantity;

                order.Items.Add(new OrderItem
                {
                    Product = product,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });
            }

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            return ToDto(order);
        }

        private static OrderDto ToDto(Order order)
        {
            var items = order.Items
                .Select(i => new OrderItemDto(i.ProductId, i.Product.Name, i.Quantity, i.UnitPrice))
                .ToList();

            return new OrderDto(order.Id, order.CreatedAt, items, items.Sum(i => i.Quantity * i.UnitPrice));
        }
    }
}
