using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Models
{
    /// <summary>
    /// Represents a single product line item inside a shopping cart.
    /// Tracks quantity and calculates subtotal.
    /// </summary>
    public class CartItem
    {
        #region Properties

        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; private set; }
        public double Subtotal => UnitPrice * Quantity;

        #endregion

        #region Constructor

        public CartItem() { }

        public CartItem(Product product, int quantity)
        {
            ProductID = product.ProductID;
            ProductName = product.Name;
            UnitPrice = product.Price;
            Quantity = quantity;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Increases the quantity of this cart item by the given amount.
        /// </summary>
        public void IncreaseQuantity(int amount)
        {
            Quantity += amount;
        }

        /// <summary>
        /// Sets the quantity of this cart item to a specific value.
        /// </summary>
        public void SetQuantity(int newQuantity)
        {
            Quantity = newQuantity;
        }

        #endregion
    }
}