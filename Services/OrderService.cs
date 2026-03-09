using OnlineShoppingSystem.Exceptions;
using OnlineShoppingSystem.Models;
using OnlineShoppingSystem.Repositories;
using OnlineShoppingSystem.Strategies;

namespace OnlineShoppingSystem.Services
{
    /// <summary>
    /// Handles all order related business logic.
    /// Uses Strategy pattern for payment processing.
    /// </summary>
    public class OrderService
    {
        #region Properties

        private readonly OrderRepository _orderRepo;
        private readonly ProductRepository _productRepo;
        private readonly IPaymentStrategy _paymentStrategy;

        #endregion

        #region Constructor

        public OrderService()
        {
            _orderRepo = DataStore.Instance.OrderRepository;
            _productRepo = DataStore.Instance.ProductRepository;
            _paymentStrategy = new WalletPayment();
        }

        #endregion

        #region Customer Methods

        /// <summary>
        /// Processes checkout — creates order, processes payment and reduces stock.
        /// Throws EmptyCartException if cart is empty.
        /// Throws InsufficientBalanceException if wallet balance is too low.
        /// </summary>
        public void Checkout(Customer customer)
        {
            if (customer.Cart.IsEmpty)
                throw new EmptyCartException();

            int orderID = _orderRepo.GetNextID();
            Order order = new Order(orderID, customer.UserID, customer.FullName, customer.Cart.Items);

            // Strategy pattern — process payment via wallet
            _paymentStrategy.ProcessPayment(customer, order.TotalAmount);

            // Save the order
            _orderRepo.Add(order);

            // Reduce stock for each item ordered
            ReduceStock(order);

            // Add to customer history and clear cart
            customer.AddOrderToHistory(order);
            customer.Cart.Clear();

            Console.WriteLine($"[OK] Order #{order.OrderID} confirmed!");
            Console.WriteLine($"     Total:  R{order.TotalAmount:F2}");
            Console.WriteLine($"     Status: {order.Status}");
        }

        /// <summary>
        /// Returns all orders placed by a specific customer.
        /// </summary>
        public IEnumerable<Order> GetOrderHistory(int customerID)
        {
            IEnumerable<Order> orders = _orderRepo.GetByCustomerID(customerID);

            if (!orders.Any())
                throw new InvalidOperationException("You have no orders yet.");

            return orders;
        }

        /// <summary>
        /// Returns a single order by ID for order tracking.
        /// </summary>
        public Order TrackOrder(int orderID)
        {
            return _orderRepo.GetById(orderID);
        }

        /// <summary>
        /// Cancels an order if it is still in a cancellable state.
        /// </summary>
        public void CancelOrder(int orderID, Customer customer)
        {
            Order order = _orderRepo.GetById(orderID);

            if (order.CustomerID != customer.UserID)
                throw new InvalidOperationException("You can only cancel your own orders.");

            order.CancelOrder();
            _orderRepo.Update(order);
        }

        #endregion

        #region Admin Methods

        /// <summary>
        /// Updates the status of an order.
        /// </summary>
        /// <summary>
        /// Updates the order status using the State Pattern.
        /// Each state controls which transitions are valid.
        /// </summary>
        public void UpdateOrderStatus(int orderID, string newStatus)
        {
            Order order = _orderRepo.GetById(orderID);

            // Restore state object since it's not saved in JSON
            order.RestoreState();

            switch (newStatus)
            {
                case "Processing": order.Process(); break;
                case "Shipped": order.Ship(); break;
                case "Delivered": order.Deliver(); break;
                case "Cancelled": order.CancelOrder(); break;
                default:
                    throw new InvalidOrderStatusException(order.Status, newStatus);
            }

            _orderRepo.Update(order);
        }

        /// <summary>
        /// Returns all orders in the system.
        /// </summary>
        public IEnumerable<Order> GetAllOrders()
        {
            return _orderRepo.GetAll();
        }

        /// <summary>
        /// Generates a sales report showing revenue and top selling products.
        /// </summary>
        public void GenerateSalesReport()
        {
            IEnumerable<Order> allOrders = _orderRepo.GetAll();
            IEnumerable<Order> deliveredOrders = allOrders.Where(o => o.Status == "Delivered");

            Console.WriteLine("╔═════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║              SALES REPORT                                               ║");
            Console.WriteLine("╠═════════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine($"║  Total Orders:     {allOrders.Count(), -53}║");
            Console.WriteLine($"║  Delivered Orders: {deliveredOrders.Count(),-53}║");
            Console.WriteLine($"║  Pending Orders:   {allOrders.Count(o => o.Status == "Pending"),-53}║");
            Console.WriteLine($"║  Cancelled Orders: {allOrders.Count(o => o.Status == "Cancelled"),-53}║");
            Console.WriteLine($"║  Total Revenue:    R{_orderRepo.GetTotalRevenue(),-52:F2}║");
            Console.WriteLine("╠═════════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║  Top Selling Products:                                                  ║");

            foreach (string product in _orderRepo.GetTopSellingProducts())
                Console.WriteLine($"║  -> {product,-68}║");

            Console.WriteLine("╚═════════════════════════════════════════════════════════════════════════╝");
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Reduces stock for each product in the order.
        /// </summary>
        private void ReduceStock(Order order)
        {
            foreach (OrderItem item in order.Items)
            {
                try
                {
                    Product product = _productRepo.GetById(item.ProductID);
                    product.ReduceStock(item.Quantity);
                    _productRepo.Update(product);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[!] Could not reduce stock for {item.ProductName}: {ex.Message}");
                }
            }
        }

        #endregion
    }
}