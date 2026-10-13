namespace InventoryApi.Services
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(int productId)
            : base($"Product with id {productId} was not found.") { }
    }

    public class InsufficientStockException : Exception
    {
        public InsufficientStockException(string productName, int requested, int available)
            : base($"Not enough stock for '{productName}': requested {requested}, available {available}.") { }
    }
}
