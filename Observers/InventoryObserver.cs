using OnlineShoppingSystem.Models;
using OnlineShoppingSystemSystem.Models;

namespace OnlineShoppingSystem.Observers
{
    /// <summary>
    /// Reduces product stock automatically when an order is placed.
    /// </summary>
    public class InventoryObserver : IOrderObserver
    {
        /// <summary>
        /// Loops through order items and reduces stock for each product.
        /// </summary>
        public void OnOrderPlaced(Order order)
        {
            foreach (OrderItem item in order.Items)
            {
                Product product = DataStore.Instance.ProductRepository.GetById(item.ProductID);
                product.ReduceStock(item.Quantity);
                DataStore.Instance.ProductRepository.Update(product);
            }
        }
    }
}