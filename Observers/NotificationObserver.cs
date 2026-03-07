using OnlineShoppingSystem.Models;
using OnlineShoppingSystemSystem.Models;

namespace OnlineShoppingSystem.Observers
{
    /// <summary>
    /// Notifies the customer when their order is successfully placed.
    /// </summary>
    public class NotificationObserver : IOrderObserver
    {
        public void OnOrderPlaced(Order order)
        {
            Console.WriteLine($"✓ Order #{order.OrderID} confirmed for {order.CustomerName}!");
            Console.WriteLine($"  Total: R{order.TotalAmount:F2}");
            Console.WriteLine($"  Status: {order.Status}");
        }
    }
}