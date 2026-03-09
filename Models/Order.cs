namespace OnlineShoppingSystem.Models
{
    /// <summary>
    /// Represents a customer order in the Online Shopping system.
    /// Tracks the full lifecycle of an order from placement to delivery.
    /// </summary>
    public class Order
    {
        #region Constants

        private const string StatusPending = "Pending";
        private const string StatusProcessing = "Processing";
        private const string StatusShipped = "Shipped";
        private const string StatusDelivered = "Delivered";
        private const string StatusCancelled = "Cancelled";

        #endregion

        #region Properties

        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public List<OrderItem> Items { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public double TotalAmount => Items.Sum(item => item.Subtotal);
        public int TotalItems => Items.Sum(item => item.Quantity);
        public bool IsCancellable => Status == StatusPending || Status == StatusProcessing;

        #endregion

        #region Constructor

        public Order()
        {
            Items = new List<OrderItem>();
        }

        public Order(int orderID, int customerID, string customerName, List<CartItem> cartItems)
        {
            OrderID = orderID;
            CustomerID = customerID;
            CustomerName = customerName;
            Status = StatusPending;
            OrderDate = DateTime.Now;
            DeliveryDate = null;
            Items = ConvertCartItemsToOrderItems(cartItems);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the order status to the next stage in the lifecycle.
        /// Throws an exception if the status transition is invalid.
        /// </summary>
        public void UpdateStatus(string newStatus)
        {
            if (!IsValidStatusTransition(newStatus))
                throw new InvalidOperationException($"Cannot change status from {Status} to {newStatus}.");

            Status = newStatus;
            Console.WriteLine($"✓ Order #{OrderID} status updated to: {Status}");

            if (newStatus == StatusDelivered)
            {
                DeliveryDate = DateTime.Now;
                Console.WriteLine($"✓ Delivered on {DeliveryDate:dd MMM yyyy HH:mm}");
            }
        }

        /// <summary>
        /// Cancels the order if it is still in a cancellable state.
        /// Throws an exception if the order cannot be cancelled.
        /// </summary>
        public void CancelOrder()
        {
            if (!IsCancellable)
                throw new InvalidOperationException($"Order #{OrderID} cannot be cancelled — current status: {Status}.");

            Status = StatusCancelled;
            Console.WriteLine($"✓ Order #{OrderID} has been cancelled.");
        }

        /// <summary>
        /// Displays a full summary of the order including all items and totals.
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
        /// Used when listing order history.
        /// </summary>
        /// Console.WriteLine($"{"ID",-5} {"Status",-11} {"TotalItems", -15} {"TotalAmount", -15} {"Date"}");
        public void DisplaySummary()
        {
            Console.WriteLine($"{OrderID, -5}  {Status,-11}  {TotalItems, -13}  R{TotalAmount, -13:F2} {OrderDate:dd MMM yyyy}");
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Converts cart items into order items at the time of purchase.
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

        /// <summary>
        /// Validates that the requested status transition is allowed.
        /// </summary>
        private bool IsValidStatusTransition(string newStatus)
        {
            return Status switch
            {
                "Pending" => newStatus == StatusProcessing || newStatus == StatusCancelled,
                "Processing" => newStatus == StatusShipped || newStatus == StatusCancelled,
                "Shipped" => newStatus == StatusDelivered,
                _ => false
            };
        }

        #endregion
    }
}