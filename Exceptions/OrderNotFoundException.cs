namespace OnlineShoppingSystem.Exceptions
{
    /// <summary>
    /// Thrown when a requested order is not found in the system.
    /// </summary>
    public class OrderNotFoundException : Exception
    {
        public int OrderID { get; }

        public OrderNotFoundException(int orderID)
            : base($"Order #{orderID} was not found.")
        {
            OrderID = orderID;
        }
    }
}