using OnlineShoppingSystem.Exceptions;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.States
{
    /// <summary>
    /// Represents the Cancelled state of an order.
    /// A cancelled order is final — no further transitions allowed.
    /// </summary>
    public class CancelledState : IOrderState
    {
        public string StateName => "Cancelled";

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