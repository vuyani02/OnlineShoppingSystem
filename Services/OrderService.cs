using OnlineShoppingSystem.Exceptions;
using OnlineShoppingSystem.Models;
using OnlineShoppingSystem.Observers;
using OnlineShoppingSystem.Repositories;
using OnlineShoppingSystem.Strategies;
using OnlineShoppingSystemSystem.Models;

namespace OnlineShoppingSystem.Services
{
    /// <summary>
    /// Handles all order related business logic.
    /// Uses Strategy pattern for payment and Observer pattern for post-order events.
    /// </summary>
    public class OrderService
    {
        #region Properties

        private readonly OrderRepository _orderRepo;
        private readonly IPaymentStrategy _paymentStrategy;
        private readonly List<IOrderObserver> _observers;

        #endregion

        #region Constructor

        public OrderService()
        {
            _orderRepo = DataStore.Instance.OrderRepository;
            _paymentStrategy = new WalletPayment();

            // Register observers — runs automatically when an order is placed
            _observers = new List<IOrderObserver>
            {
                new InventoryObserver(),
                new NotificationObserver()
            };
        }

        #endregion

        #region Customer Methods

        /// <summary>
        /// Processes checkout — creates order, processes payment and notifies observers.
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

            // Observer pattern — notify all observers
            NotifyObservers(order);

            // Add to customer history and clear cart
            customer.AddOrderToHistory(order);
            customer.Cart.Clear();
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
        public void UpdateOrderStatus(int orderID, string newStatus)
        {
            Order order = _orderRepo.GetById(orderID);
            order.UpdateStatus(newStatus);
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

            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║              SALES REPORT                    ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine($"  Total Orders:     {allOrders.Count()}");
            Console.WriteLine($"  Delivered Orders: {deliveredOrders.Count()}");
            Console.WriteLine($"  Pending Orders:   {allOrders.Count(o => o.Status == "Pending")}");
            Console.WriteLine($"  Cancelled Orders: {allOrders.Count(o => o.Status == "Cancelled")}");
            Console.WriteLine($"  Total Revenue:    R{_orderRepo.GetTotalRevenue():F2}");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("  Top Selling Products:");

            foreach (string product in _orderRepo.GetTopSellingProducts())
            {
                Console.WriteLine($"  → {product}");
            }

            Console.WriteLine("╚══════════════════════════════════════════════╝");
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Notifies all registered observers when an order is placed.
        /// </summary>
        private void NotifyObservers(Order order)
        {
            foreach (IOrderObserver observer in _observers)
            {
                observer.OnOrderPlaced(order);
            }
        }

        #endregion
    }
}