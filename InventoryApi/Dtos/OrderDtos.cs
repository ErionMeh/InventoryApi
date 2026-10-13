using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Dtos
{
    public class OrderItemCreateDto
    {
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

    public class OrderCreateDto
    {
        [Required, MinLength(1)]
        public List<OrderItemCreateDto> Items { get; set; } = new();
    }

    public record OrderItemDto(int ProductId, string ProductName, int Quantity, decimal UnitPrice);

    public record OrderDto(int Id, DateTime CreatedAt, List<OrderItemDto> Items, decimal Total);
}
