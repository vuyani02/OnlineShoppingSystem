namespace OnlineShoppingSystem.Exceptions
{
    /// <summary>
    /// Thrown when a customer does not have sufficient wallet balance to complete a purchase.
    /// </summary>
    public class InsufficientBalanceException : Exception
    {
        public double RequiredAmount { get; }
        public double AvailableAmount { get; }

        public InsufficientBalanceException(double required, double available)
            : base($"Insufficient balance. Required: R{required:F2}, Available: R{available:F2}. Please top up your wallet.")
        {
            RequiredAmount = required;
            AvailableAmount = available;
        }
    }
}