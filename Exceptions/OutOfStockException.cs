namespace OnlineShoppingSystem.Exceptions
{
    /// <summary>
    /// Thrown when a customer attempts to purchase a product with insufficient stock.
    /// </summary>
    public class OutOfStockException : Exception
    {
        public string ProductName { get; }
        public int AvailableStock { get; }

        public OutOfStockException(string productName, int availableStock)
            : base($"{productName} does not have enough stock. Available: {availableStock}.")
        {
            ProductName = productName;
            AvailableStock = availableStock;
        }
    }
}