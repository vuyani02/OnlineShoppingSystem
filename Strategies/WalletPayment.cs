using OnlineShoppingSystem.Models;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Strategies
{
    /// <summary>
    /// Processes payment using the customer's wallet balance.
    /// </summary>
    public class WalletPayment : IPaymentStrategy
    {
        /// <summary>
        /// Deducts the amount from the customer's wallet.
        /// Throws InsufficientBalanceException if balance is too low.
        /// </summary>
        public void ProcessPayment(Customer customer, double amount)
        {
            customer.DeductBalance(amount);
            Console.WriteLine($"✓ R{amount:F2} paid from wallet.");
            Console.WriteLine($"  Remaining balance: R{customer.WalletBalance:F2}");
        }
    }
}