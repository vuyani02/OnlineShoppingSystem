namespace OnlineShoppingSystemSystem.Models
{
    /// <summary>
    /// Represents a single product line item within an order.
    /// Captures the price at the time of purchase so price changes don't affect old orders.
    /// </summary>
    public class OrderItem
    {
        #region Properties

        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double Subtotal => UnitPrice * Quantity;

        #endregion

        #region Constructor

        public OrderItem(int productID, string productName, double unitPrice, int quantity)
        {
            ProductID = productID;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Displays a single order item line.
        /// </summary>
        public void DisplayInfo()
        {
            Console.WriteLine($"{ProductName,-25} x{Quantity}  R{UnitPrice:F2} each  =  R{Subtotal:F2}");
        }

        #endregion
    }
}