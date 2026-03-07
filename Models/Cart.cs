namespace OnlineShoppingSystemSystem.Models
{
    /// <summary>
    /// Represents a shopping cart belonging to a customer.
    /// Manages cart items and calculates totals.
    /// </summary>
    public class Cart
    {
        #region Properties

        public int CartID { get; set; }
        public int CustomerID { get; set; }
        public List<CartItem> Items { get; private set; }
        public double TotalPrice => Items.Sum(item => item.Subtotal);
        public int TotalItems => Items.Sum(item => item.Quantity);
        public bool IsEmpty => Items.Count == 0;

        #endregion

        #region Constructor

        public Cart(int customerID)
        {
            CustomerID = customerID;
            Items = new List<CartItem>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a product to the cart.
        /// If product already exists in cart, increases the quantity instead.
        /// </summary>
        public void AddItem(Product product, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            if (product.Stock < quantity)
                throw new OutOfStockException(product.Name, product.Stock);

            CartItem existingItem = Items.FirstOrDefault(i => i.ProductID == product.ProductID);

            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
            }
            else
            {
                Items.Add(new CartItem(product, quantity));
            }

            Console.WriteLine($"✓ {product.Name} x{quantity} added to cart.");
        }

        /// <summary>
        /// Removes a product completely from the cart by product ID.
        /// </summary>
        public void RemoveItem(int productID)
        {
            CartItem item = GetItemByProductID(productID);

            Items.Remove(item);
            Console.WriteLine($"✓ {item.ProductName} removed from cart.");
        }

        /// <summary>
        /// Updates the quantity of an existing cart item.
        /// Removes the item if quantity is set to zero.
        /// </summary>
        public void UpdateQuantity(int productID, int newQuantity)
        {
            if (newQuantity < 0)
                throw new ArgumentException("Quantity cannot be negative.");

            CartItem item = GetItemByProductID(productID);

            if (newQuantity == 0)
            {
                RemoveItem(productID);
                return;
            }

            item.SetQuantity(newQuantity);
            Console.WriteLine($"✓ {item.ProductName} quantity updated to {newQuantity}.");
        }

        /// <summary>
        /// Clears all items from the cart after a successful order.
        /// </summary>
        public void Clear()
        {
            Items.Clear();
            Console.WriteLine("✓ Cart cleared.");
        }

        /// <summary>
        /// Displays all items in the cart with their subtotals and the overall total.
        /// </summary>
        public void DisplayCart()
        {
            if (IsEmpty)
            {
                Console.WriteLine("Your cart is empty.");
                return;
            }

            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║               YOUR SHOPPING CART             ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine($"{"Product",-20} {"Qty",-5} {"Price",-10} {"Subtotal",-10}");
            Console.WriteLine(new string('─', 46));

            foreach (CartItem item in Items)
            {
                Console.WriteLine($"{item.ProductName,-20} {item.Quantity,-5} R{item.UnitPrice,-9:F2} R{item.Subtotal:F2}");
            }

            Console.WriteLine(new string('─', 46));
            Console.WriteLine($"{"Total Items:",-20} {TotalItems}");
            Console.WriteLine($"{"Total Price:",-20} R{TotalPrice:F2}");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Finds a cart item by product ID.
        /// Throws an exception if the item is not found.
        /// </summary>
        private CartItem GetItemByProductID(int productID)
        {
            CartItem item = Items.FirstOrDefault(i => i.ProductID == productID);

            if (item == null)
                throw new InvalidOperationException($"Product ID {productID} not found in cart.");

            return item;
        }

        #endregion
    }
}