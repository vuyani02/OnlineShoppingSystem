using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Models
{
    /// <summary>
    /// Represents a customer review on a product.
    /// </summary>
    public class Review
    {
        #region Properties

        public int ReviewID { get; set; }
        public int ProductID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public double Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }

        #endregion

        #region Constructor

        public Review() { }

        public Review(int reviewID, int productID, int customerID, string customerName, double rating, string comment)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");

            ReviewID = reviewID;
            ProductID = productID;
            CustomerID = customerID;
            CustomerName = customerName;
            Rating = rating;
            Comment = comment;
            ReviewDate = DateTime.Now;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Displays the review details to the console.
        /// </summary>
        public void DisplayInfo()
        {
            Console.WriteLine($"  ⭐ {Rating}/5 — {CustomerName}");
            Console.WriteLine($"  {Comment}");
            Console.WriteLine($"  {ReviewDate:dd MMM yyyy}");
        }

        #endregion
    }
}