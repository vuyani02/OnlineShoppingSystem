namespace OnlineShoppingSystem.Models
{
    /// <summary>
    /// Represents a discount tier with its thresholds and discount percentage.
    /// </summary>
    public class DiscountTier
    {
        #region Properties

        public string TierName { get; set; }
        public int MinOrders { get; set; }
        public double MinAmountSpent { get; set; }
        public double DiscountPercent { get; set; }

        #endregion

        #region Constructor

        public DiscountTier(string tierName, int minOrders, double minAmountSpent, double discountPercent)
        {
            TierName = tierName;
            MinOrders = minOrders;
            MinAmountSpent = minAmountSpent;
            DiscountPercent = discountPercent;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks if a customer qualifies for this tier
        /// based on their order count or total amount spent.
        /// </summary>
        public bool IsEligible(int totalOrders, double totalAmountSpent)
        {
            return totalOrders >= MinOrders || totalAmountSpent >= MinAmountSpent;
        }

        /// <summary>
        /// Displays the tier details to the console.
        /// </summary>
        public void DisplayInfo()
        {
            Console.WriteLine($"  {TierName,-15} {DiscountPercent}% off — {MinOrders}+ orders OR R{MinAmountSpent:F2}+ spent");
        }

        #endregion
    }
}