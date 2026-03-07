using OnlineShoppingSystem.Exceptions;

namespace OnlineShoppingSystemSystem.Models
{
    /// <summary>
    /// Represents a product in the Online Shopping system.
    /// Tracks product details, stock levels and customer reviews.
    /// </summary>
    public class Product
    {
        #region Constants

        private const int LowStockThreshold = 5;

        #endregion

        #region Properties

        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public int Stock { get; private set; }
        public List<Review> Reviews { get; private set; }
        public bool IsAvailable => Stock > 0;
        public bool IsLowStock => Stock <= LowStockThreshold && Stock > 0;
        public double AverageRating => Reviews.Count == 0 ? 0 : Reviews.Average(r => r.Rating);

        #endregion

        #region Constructor

        public Product(int productID, string name, string description, string category, double price, int stock)
        {
            ProductID = productID;
            Name = name;
            Description = description;
            Category = category;
            Price = price;
            Stock = stock;
            Reviews = new List<Review>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reduces stock when a customer places an order.
        /// Throws OutOfStockException if stock is insufficient.
        /// </summary>
        public void ReduceStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            if (Stock < quantity)
                throw new OutOfStockException(Name, Stock);

            Stock -= quantity;

            if (IsLowStock)
                Console.WriteLine($"⚠️  Low stock alert: {Name} has {Stock} items remaining.");
        }

        /// <summary>
        /// Increases stock when an administrator restocks a product.
        /// </summary>
        public void RestockProduct(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Restock quantity must be greater than zero.");

            Stock += quantity;
            Console.WriteLine($"✓ {Name} restocked. New stock: {Stock}");
        }

        /// <summary>
        /// Adds a customer review to the product.
        /// </summary>
        public void AddReview(Review review)
        {
            Reviews.Add(review);
        }

        /// <summary>
        /// Displays full product details including stock and rating.
        /// </summary>
        public void DisplayInfo()
        {
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine($"  ID:          {ProductID}");
            Console.WriteLine($"  Name:        {Name}");
            Console.WriteLine($"  Category:    {Category}");
            Console.WriteLine($"  Description: {Description}");
            Console.WriteLine($"  Price:       R{Price:F2}");
            Console.WriteLine($"  Stock:       {Stock} {(IsLowStock ? "⚠️  Low Stock" : "")}");
            Console.WriteLine($"  Rating:      ⭐ {AverageRating:F1} ({Reviews.Count} reviews)");
            Console.WriteLine($"  Available:   {(IsAvailable ? "Yes" : "Out of Stock")}");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
        }

        /// <summary>
        /// Displays a compact single line summary of the product.
        /// Used when browsing the product catalog.
        /// </summary>
        public void DisplaySummary()
        {
            string stockStatus = IsAvailable ? $"{Stock} in stock" : "Out of Stock";
            string lowStock = IsLowStock ? "⚠️" : "";
            Console.WriteLine($"[{ProductID}] {Name,-25} R{Price,-10:F2} ⭐{AverageRating:F1}  {stockStatus} {lowStock}");
        }

        #endregion
    }
}