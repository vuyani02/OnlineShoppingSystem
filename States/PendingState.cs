using OnlineShoppingSystem.Exceptions;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.States
{
    /// <summary>
    /// Represents the Pending state of an order.
    /// A pending order can be processed or cancelled only.
    /// </summary>
    public class PendingState : IOrderState
    {
        public string StateName => "Pending";

        /// <summary>
        /// Moves the order to Processing state.
        /// </summary>
        public void Process(Order order)
        {
            order.SetState(new ProcessingState());
            Console.WriteLine($"[OK] Order #{order.OrderID} is now Processing.");
        }

        /// <summary>
        /// Cannot ship a pending order.
        /// </summary>
        public void Ship(Order order)
        {
            throw new InvalidOrderStatusException(StateName, "Shipped");
        }

        /// <summary>
        /// Cannot deliver a pending order.
        /// </summary>
        public void Deliver(Order order)
        {
            throw new InvalidOrderStatusException(StateName, "Delivered");
        }

        /// <summary>
        /// Cancels the pending order.
        /// </summary>
        public void Cancel(Order order)
        {
            order.SetState(new CancelledState());
            Console.WriteLine($"[OK] Order #{order.OrderID} has been Cancelled.");
        }
    }
}