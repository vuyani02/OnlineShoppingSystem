using System.Text.Json.Serialization;
using OnlineShoppingSystem.States;

namespace OnlineShoppingSystem.Models
{
    /// <summary>
    /// Represents a customer order in the Online Shopping system.
    /// Uses the State Pattern to manage order lifecycle transitions.
    /// </summary>
    public class Order
    {
        #region Properties

        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public List<OrderItem> Items { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }

        [JsonIgnore]
        public double TotalAmount => Items.Sum(item => item.Subtotal);

        [JsonIgnore]
        public int TotalItems => Items.Sum(item => item.Quantity);

        [JsonIgnore]
        public bool IsCancellable => Status == "Pending" || Status == "Processing";

        // Current state object — not saved to JSON
        [JsonIgnore]
        private IOrderState _currentState;

        #endregion

        #region Constructor

        // Parameterless constructor for JSON deserialization
        public Order()
        {
            Items = new List<OrderItem>();
        }

        public Order(int orderID, int customerID, string customerName, List<CartItem> cartItems)
        {
            OrderID = orderID;
            CustomerID = customerID;
            CustomerName = customerName;
            OrderDate = DateTime.Now;
            DeliveryDate = null;
            Items = ConvertCartItemsToOrderItems(cartItems);

            // Set initial state
            SetState(new PendingState());
        }

        #endregion

        #region State Methods

        /// <summary>
        /// Sets the current state and updates the Status string.
        /// </summary>
        public void SetState(IOrderState state)
        {
            _currentState = state;
            Status = state.StateName;
        }

        /// <summary>
        /// Sets the delivery date when order is delivered.
        /// </summary>
        public void SetDeliveryDate(DateTime date)
        {
            DeliveryDate = date;
        }

        /// <summary>
        /// Loads the correct state object based on the saved Status string.
        /// Called after loading from JSON since state objects aren't serialized.
        /// </summary>
        public void RestoreState()
        {
            _currentState = Status switch
            {
                "Pending" => new PendingState(),
                "Processing" => new ProcessingState(),
                "Shipped" => new ShippedState(),
                "Delivered" => new DeliveredState(),
                "Cancelled" => new CancelledState(),
                _ => new PendingState()
            };
        }

        #endregion

        #region Order Lifecycle Methods

        /// <summary>
        /// Transitions the order to Processing state.
        /// </summary>
        public void Process()
        {
            EnsureStateLoaded();
            _currentState.Process(this);
        }

        /// <summary>
        /// Transitions the order to Shipped state.
        /// </summary>
        public void Ship()
        {
            EnsureStateLoaded();
            _currentState.Ship(this);
        }

        /// <summary>
        /// Transitions the order to Delivered state.
        /// </summary>
        public void Deliver()
        {
            EnsureStateLoaded();
            _currentState.Deliver(this);
        }

        /// <summary>
        /// Cancels the order if current state allows it.
        /// </summary>
        public void CancelOrder()
        {
            EnsureStateLoaded();
            _currentState.Cancel(this);
        }

        #endregion

        #region Display Methods

        /// <summary>
        /// Displays full order details including all items and totals.
        /// </summary>
        public void DisplayInfo()
        {
            Console.WriteLine($"  Order ID:   #{OrderID}");
            Console.WriteLine($"  Customer:   {CustomerName}");
            Console.WriteLine($"  Date:       {OrderDate:dd MMM yyyy HH:mm}");
            Console.WriteLine($"  Status:     {Status}");

            if (DeliveryDate.HasValue)
                Console.WriteLine($"  Delivered:  {DeliveryDate:dd MMM yyyy HH:mm}");

            Console.WriteLine();
            Console.WriteLine($"  {"Product",-25} {"Qty",-5} {"Price",-10} {"Subtotal"}");
            Console.WriteLine($"  {new string('─', 50)}");

            foreach (OrderItem item in Items)
            {
                Console.WriteLine($"  {item.ProductName,-25} {item.Quantity,-5} R{item.UnitPrice,-9:F2} R{item.Subtotal:F2}");
            }

            Console.WriteLine($"  {new string('─', 50)}");
            Console.WriteLine($"  {"Total Items:",-25} {TotalItems}");
            Console.WriteLine($"  {"Total Amount:",-25} R{TotalAmount:F2}");
        }

        /// <summary>
        /// Displays a compact single line summary of the order.
        /// </summary>
        public void DisplaySummary()
        {
            Console.WriteLine($"{OrderID,-5}  {Status,-11}  {TotalItems,-13}  R{TotalAmount,-13:F2} {OrderDate:dd MMM yyyy}");
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Ensures state is loaded — restores from Status string if needed.
        /// This handles orders loaded from JSON where state object is null.
        /// </summary>
        private void EnsureStateLoaded()
        {
            if (_currentState == null)
                RestoreState();
        }

        /// <summary>
        /// Converts cart items into order items at time of purchase.
        /// Preserves the price at the time of purchase.
        /// </summary>
        private List<OrderItem> ConvertCartItemsToOrderItems(List<CartItem> cartItems)
        {
            return cartItems.Select(cartItem => new OrderItem(
                cartItem.ProductID,
                cartItem.ProductName,
                cartItem.UnitPrice,
                cartItem.Quantity
            )).ToList();
        }

        #endregion
    }
}