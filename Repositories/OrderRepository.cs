using OnlineShoppingSystem.Exceptions;
using OnlineShoppingSystem.Models;
using OnlineShoppingSystem.Repositories;
using OnlineShoppingSystem.Models;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OnlineShoppingSystem.Repositories
{
    /// <summary>
    /// Handles all data operations for orders.
    /// Reads and writes to orders.json.
    /// </summary>
    public class OrderRepository : IRepository<Order>
    {
        #region Constants

        private const string FilePath = "data/orders.json";

        #endregion

        #region Properties

        private List<Order> _orders;

        #endregion

        #region Constructor

        public OrderRepository()
        {
            _orders = LoadFromJson();
        }

        #endregion

        #region IRepository Methods

        /// <summary>
        /// Adds a new order to the system.
        /// </summary>
        public void Add(Order order)
        {
            _orders.Add(order);
            SaveToJson();
        }

        /// <summary>
        /// Removes an order from the system.
        /// </summary>
        public void Remove(Order order)
        {
            _orders.Remove(order);
            SaveToJson();
        }

        /// <summary>
        /// Updates an existing order.
        /// </summary>
        public void Update(Order updatedOrder)
        {
            Order existing = GetById(updatedOrder.OrderID);
            _orders.Remove(existing);
            _orders.Add(updatedOrder);
            SaveToJson();
        }

        /// <summary>
        /// Returns an order by its ID.
        /// Throws OrderNotFoundException if not found.
        /// </summary>
        public Order GetById(int id)
        {
            Order order = _orders.FirstOrDefault(o => o.OrderID == id);

            if (order == null)
                throw new OrderNotFoundException(id);

            return order;
        }

        /// <summary>
        /// Returns all orders in the system.
        /// </summary>
        public IEnumerable<Order> GetAll()
        {
            return _orders;
        }

        #endregion

        #region Extra Methods

        /// <summary>
        /// Returns all orders placed by a specific customer.
        /// </summary>
        public IEnumerable<Order> GetByCustomerID(int customerID)
        {
            return _orders
                .Where(o => o.CustomerID == customerID)
                .OrderByDescending(o => o.OrderDate);
        }

        /// <summary>
        /// Returns all orders with a specific status.
        /// </summary>
        public IEnumerable<Order> GetByStatus(string status)
        {
            return _orders.Where(o => o.Status == status);
        }

        /// <summary>
        /// Returns total revenue from all delivered orders.
        /// </summary>
        public double GetTotalRevenue()
        {
            return _orders
                .Where(o => o.Status == "Delivered")
                .Sum(o => o.TotalAmount);
        }

        /// <summary>
        /// Returns the next available order ID.
        /// </summary>
        public int GetNextID()
        {
            return _orders.Count == 0 ? 1 : _orders.Max(o => o.OrderID) + 1;
        }

        /// <summary>
        /// Returns the top selling products based on quantity ordered.
        /// </summary>
        public IEnumerable<string> GetTopSellingProducts(int top = 5)
        {
            return _orders
                .Where(o => o.Status == "Delivered")
                .SelectMany(o => o.Items)
                .GroupBy(item => item.ProductName)
                .OrderByDescending(group => group.Sum(item => item.Quantity))
                .Take(top)
                .Select(group => $"{group.Key} — {group.Sum(item => item.Quantity)} sold");
        }

        #endregion

        #region JSON Methods

        /// <summary>
        /// Saves the current list of orders to the JSON file.
        /// </summary>
        public void SaveToJson()
        {
            EnsureDataFolderExists();
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(_orders, options);
            File.WriteAllText(FilePath, json);
        }

        /// <summary>
        /// Loads orders from the JSON file.
        /// Returns empty list if file does not exist.
        /// </summary>
        private List<Order> LoadFromJson()
        {
            if (!File.Exists(FilePath))
                return new List<Order>();

            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Order>>(json) ?? new List<Order>();
        }

        #endregion

        #region Private Helper Methods

        private void EnsureDataFolderExists()
        {
            if (!Directory.Exists("data"))
                Directory.CreateDirectory("data");
        }

        #endregion
    }
}