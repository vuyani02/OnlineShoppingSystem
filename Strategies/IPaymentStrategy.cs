using OnlineShoppingSystem.Models;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Strategies
{
    /// <summary>
    /// Defines the contract for all payment strategies.
    /// </summary>
    public interface IPaymentStrategy
    {
        void ProcessPayment(Customer customer, double amount);
    }
}