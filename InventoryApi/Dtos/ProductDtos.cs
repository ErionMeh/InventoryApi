using System.ComponentModel.DataAnnotations;

namespace InventoryApi.Dtos
{
    public class ProductCreateDto
    {
        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Sku { get; set; } = string.Empty;

        [Range(0.01, 1000000)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int QuantityInStock { get; set; }
    }

    public class ProductUpdateDto
    {
        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, 1000000)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int QuantityInStock { get; set; }
    }

    public record ProductDto(int Id, string Name, string Sku, decimal Price, int QuantityInStock);
}
