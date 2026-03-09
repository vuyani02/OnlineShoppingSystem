using OnlineShoppingSystem.Exceptions;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.States
{
    /// <summary>
    /// Represents the Delivered state of an order.
    /// A delivered order is final — no further transitions allowed.
    /// </summary>
    public class DeliveredState : IOrderState
    {
        public string StateName => "Delivered";

        public void Process(Order order)
        {
            throw new InvalidOrderStatusException(StateName, "Processing");
        }

        public void Ship(Order order)
        {
            throw new InvalidOrderStatusException(StateName, "Shipped");
        }

        public void Deliver(Order order)
        {
            throw new InvalidOrderStatusException(StateName, "Delivered");
        }

        public void Cancel(Order order)
        {
            throw new InvalidOrderStatusException(StateName, "Cancelled");
        }
    }
}