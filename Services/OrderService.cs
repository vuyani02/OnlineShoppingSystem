using OnlineShoppingSystem.Exceptions;
using OnlineShoppingSystem.Models;
using OnlineShoppingSystem.Repositories;
using OnlineShoppingSystem.Strategies;

namespace OnlineShoppingSystem.Services
{
    /// <summary>
    /// Handles all order related business logic.
    /// Uses Strategy pattern for payment and DiscountService for loyalty discounts.
    /// </summary>
    public class OrderService
    {
        #region Properties

        private readonly OrderRepository _orderRepo;
        private readonly ProductRepository _productRepo;
        private readonly IPaymentStrategy _paymentStrategy;
        private readonly DiscountService _discountService;

        #endregion

        #region Constructor

        public OrderService()
        {
            _orderRepo = DataStore.Instance.OrderRepository;
            _productRepo = DataStore.Instance.ProductRepository;
            _paymentStrategy = new WalletPayment();
            _discountService = new DiscountService();
        }

        #endregion

        #region Customer Methods

        /// <summary>
        /// Processes checkout — applies loyalty discount, creates order,
        /// processes payment and reduces stock.
        /// </summary>
        public void Checkout(Customer customer)
        {
            if (customer.Cart.IsEmpty)
                throw new EmptyCartException();

            // Calculate discount based on order history
            double discountPercent = _discountService.GetDiscountPercent(customer.UserID);
            double discountAmount = _discountService.CalculateDiscountAmount(customer.Cart.TotalPrice, discountPercent);

            int orderID = _orderRepo.GetNextID();
            Order order = new Order(orderID, customer.UserID, customer.FullName, customer.Cart.Items, discountPercent, discountAmount);

            // Process payment with discounted total
            _paymentStrategy.ProcessPayment(customer, order.TotalAmount);

            // Save order
            _orderRepo.Add(order);

            // Reduce stock
            ReduceStock(order);

            // Add to history and clear cart
            customer.AddOrderToHistory(order);
            customer.Cart.Clear();

            Console.WriteLine($"\n[OK] Order #{order.OrderID} placed successfully!");
            Console.WriteLine($"     Subtotal:  R{order.Subtotal:F2}");

            if (discountPercent > 0)
                Console.WriteLine($"     Discount:  -{discountPercent}% (-R{discountAmount:F2})");

            Console.WriteLine($"     Total:     R{order.TotalAmount:F2}");
            Console.WriteLine($"     Status:    {order.Status}");
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
        /// Returns a single order by ID for tracking.
        /// </summary>
        public Order TrackOrder(int orderID)
        {
            return _orderRepo.GetById(orderID);
        }

        /// <summary>
        /// Cancels an order if it belongs to the customer and is cancellable.
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
        /// Updates the order status using the State Pattern.
        /// </summary>
        public void UpdateOrderStatus(int orderID, string newStatus)
        {
            Order order = _orderRepo.GetById(orderID);
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
            Console.WriteLine($"║  Total Orders:     {allOrders.Count(),-53}║");
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

        /// <summary>
        /// Returns the DiscountService for use in Program.cs menus.
        /// </summary>
        public DiscountService GetDiscountService()
        {
            return _discountService;
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