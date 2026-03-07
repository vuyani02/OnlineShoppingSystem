namespace OnlineShoppingSystem.Exceptions
{
    /// <summary>
    /// Thrown when a requested product is not found in the system.
    /// </summary>
    public class ProductNotFoundException : Exception
    {
        public int ProductID { get; }

        public ProductNotFoundException(int productID)
            : base($"Product with ID {productID} was not found.")
        {
            ProductID = productID;
        }
    }
}