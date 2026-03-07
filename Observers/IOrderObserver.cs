using OnlineShoppingSystem.Models;
using OnlineShoppingSystemSystem.Models;

namespace OnlineShoppingSystem.Observers
{
    /// <summary>
    /// Defines the contract for all order observers.
    /// Any class that wants to react to order events must implement this.
    /// </summary>
    public interface IOrderObserver
    {
        void OnOrderPlaced(Order order);
    }
}