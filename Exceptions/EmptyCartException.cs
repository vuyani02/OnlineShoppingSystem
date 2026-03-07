namespace OnlineShoppingSystem.Exceptions
{
    /// <summary>
    /// Thrown when a customer attempts to checkout with an empty cart.
    /// </summary>
    public class EmptyCartException : Exception
    {
        public EmptyCartException()
            : base("Your cart is empty. Add items before checking out.")
        {
        }
    }
}