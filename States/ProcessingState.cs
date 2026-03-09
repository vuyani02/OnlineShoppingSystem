using OnlineShoppingSystem.Exceptions;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.States
{
    /// <summary>
    /// Represents the Processing state of an order.
    /// A processing order can be shipped or cancelled only.
    /// </summary>
    public class ProcessingState : IOrderState
    {
        public string StateName => "Processing";

        /// <summary>
        /// Cannot process an already processing order.
        /// </summary>
        public void Process(Order order)
        {
            throw new InvalidOrderStatusException(StateName, "Processing");
        }

        /// <summary>
        /// Moves the order to Shipped state.
        /// </summary>
        public void Ship(Order order)
        {
            order.SetState(new ShippedState());
            Console.WriteLine($"[OK] Order #{order.OrderID} has been Shipped.");
        }

        /// <summary>
        /// Cannot deliver before shipping.
        /// </summary>
        public void Deliver(Order order)
        {
            throw new InvalidOrderStatusException(StateName, "Delivered");
        }

        /// <summary>
        /// Cancels the processing order.
        /// </summary>
        public void Cancel(Order order)
        {
            order.SetState(new CancelledState());
            Console.WriteLine($"[OK] Order #{order.OrderID} has been Cancelled.");
        }
    }
}