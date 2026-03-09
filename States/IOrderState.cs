using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.States
{
    /// <summary>
    /// Defines the contract for all order states.
    /// Each state controls which transitions are allowed.
    /// </summary>
    public interface IOrderState
    {
        void Process(Order order);
        void Ship(Order order);
        void Deliver(Order order);
        void Cancel(Order order);
        string StateName { get; }
    }
}