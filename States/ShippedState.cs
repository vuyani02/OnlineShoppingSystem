using OnlineShoppingSystem.Exceptions;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.States
{
    /// <summary>
    /// Represents the Shipped state of an order.
    /// A shipped order can only be delivered — cannot be cancelled.
    /// </summary>
    public class ShippedState : IOrderState
    {
        public string StateName => "Shipped";

        /// <summary>
        /// Cannot process an already shipped order.
        /// </summary>
        public void Process(Order order)
        {
            throw new InvalidOrderStatusException(StateName, "Processing");
        }

        /// <summary>
        /// Cannot ship an already shipped order.
        /// </summary>
        public void Ship(Order order)
        {
            throw new InvalidOrderStatusException(StateName, "Shipped");
        }

        /// <summary>
        /// Moves the order to Delivered state.
        /// </summary>
        public void Deliver(Order order)
        {
            order.SetState(new DeliveredState());
            order.SetDeliveryDate(DateTime.Now);
            Console.WriteLine($"[OK] Order #{order.OrderID} has been Delivered.");
            Console.WriteLine($"     Delivered on: {order.DeliveryDate:dd MMM yyyy HH:mm}");
        }

        /// <summary>
        /// Cannot cancel an already shipped order.
        /// </summary>
        public void Cancel(Order order)
        {
            throw new InvalidOrderStatusException(StateName, "Cancelled");
        }
    }
}