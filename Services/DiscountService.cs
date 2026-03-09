using OnlineShoppingSystem.Models;
using OnlineShoppingSystem.Repositories;

namespace OnlineShoppingSystem.Services
{
    /// <summary>
    /// Calculates the discount tier and discount amount for a customer
    /// based on their order history.
    /// </summary>
    public class DiscountService
    {
        #region Constants

        private const double Tier1Percent = 5.00;
        private const double Tier2Percent = 10.00;
        private const double Tier3Percent = 15.00;

        private const int Tier1MinOrders = 3;
        private const int Tier2MinOrders = 5;
        private const int Tier3MinOrders = 10;

        private const double Tier1MinSpent = 2000.00;
        private const double Tier2MinSpent = 5000.00;
        private const double Tier3MinSpent = 10000.00;

        #endregion

        #region Properties

        private readonly OrderRepository _orderRepo;
        private readonly List<DiscountTier> _tiers;

        #endregion

        #region Constructor

        public DiscountService()
        {
            _orderRepo = DataStore.Instance.OrderRepository;

            // Define tiers from lowest to highest — order matters
            _tiers = new List<DiscountTier>
            {
                new DiscountTier("Bronze", Tier1MinOrders, Tier1MinSpent, Tier1Percent),
                new DiscountTier("Silver", Tier2MinOrders, Tier2MinSpent, Tier2Percent),
                new DiscountTier("Gold",   Tier3MinOrders, Tier3MinSpent, Tier3Percent),
            };
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the discount percentage a customer qualifies for
        /// based on their completed order history.
        /// Returns 0 if no discount applies.
        /// </summary>
        public double GetDiscountPercent(int customerID)
        {
            int totalOrders = GetTotalOrders(customerID);
            double totalAmountSpent = GetTotalAmountSpent(customerID);

            // Find the highest tier the customer qualifies for
            DiscountTier qualifiedTier = _tiers
                .Where(t => t.IsEligible(totalOrders, totalAmountSpent))
                .OrderByDescending(t => t.DiscountPercent)
                .FirstOrDefault();

            return qualifiedTier?.DiscountPercent ?? 0.00;
        }

        /// <summary>
        /// Calculates the discount amount for a given order total.
        /// </summary>
        public double CalculateDiscountAmount(double orderTotal, double discountPercent)
        {
            return orderTotal * (discountPercent / 100);
        }

        /// <summary>
        /// Returns the discount tier name a customer currently belongs to.
        /// Returns "No Discount" if customer has not reached any tier.
        /// </summary>
        public string GetTierName(int customerID)
        {
            int totalOrders = GetTotalOrders(customerID);
            double totalAmountSpent = GetTotalAmountSpent(customerID);

            DiscountTier qualifiedTier = _tiers
                .Where(t => t.IsEligible(totalOrders, totalAmountSpent))
                .OrderByDescending(t => t.DiscountPercent)
                .FirstOrDefault();

            return qualifiedTier?.TierName ?? "No Discount";
        }

        /// <summary>
        /// Returns the total number of completed orders for a customer.
        /// </summary>
        public int GetTotalOrders(int customerID)
        {
            return _orderRepo
                .GetByCustomerID(customerID)
                .Count(o => o.Status == "Delivered");
        }

        /// <summary>
        /// Returns the total amount spent by a customer on delivered orders.
        /// </summary>
        public double GetTotalAmountSpent(int customerID)
        {
            return _orderRepo
                .GetByCustomerID(customerID)
                .Where(o => o.Status == "Delivered")
                .Sum(o => o.TotalAmount);
        }

        /// <summary>
        /// Displays all available discount tiers to the console.
        /// </summary>
        public void DisplayAllTiers()
        {
            Console.WriteLine("=== DISCOUNT TIERS ===\n");
            Console.WriteLine($"  {"Tier",-15} {"Discount",-10} {"Min Orders",-15} {"Min Spent"}");
            Console.WriteLine($"  {new string('─', 55)}");

            foreach (DiscountTier tier in _tiers)
                tier.DisplayInfo();
        }

        /// <summary>
        /// Displays the customer's current discount status.
        /// </summary>
        public void DisplayCustomerDiscount(int customerID)
        {
            int totalOrders = GetTotalOrders(customerID);
            double totalAmountSpent = GetTotalAmountSpent(customerID);
            double discountPercent = GetDiscountPercent(customerID);
            string tierName = GetTierName(customerID);

            Console.WriteLine("=== YOUR DISCOUNT STATUS ===\n");
            Console.WriteLine($"  Tier:           {tierName}");
            Console.WriteLine($"  Discount:       {discountPercent}%");
            Console.WriteLine($"  Orders placed:  {totalOrders}");
            Console.WriteLine($"  Total spent:    R{totalAmountSpent:F2}");

            // Show progress to next tier
            DisplayNextTierProgress(totalOrders, totalAmountSpent);
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Shows how close the customer is to the next discount tier.
        /// </summary>
        private void DisplayNextTierProgress(int totalOrders, double totalAmountSpent)
        {
            DiscountTier nextTier = _tiers
                .Where(t => !t.IsEligible(totalOrders, totalAmountSpent))
                .OrderBy(t => t.DiscountPercent)
                .FirstOrDefault();

            if (nextTier == null)
            {
                Console.WriteLine("\n  [OK] You are on the highest discount tier!");
                return;
            }

            int ordersNeeded = Math.Max(0, nextTier.MinOrders - totalOrders);
            double amountNeeded = Math.Max(0, nextTier.MinAmountSpent - totalAmountSpent);

            Console.WriteLine($"\n  Next tier: {nextTier.TierName} ({nextTier.DiscountPercent}% off)");

            if (ordersNeeded > 0)
                Console.WriteLine($"  Place {ordersNeeded} more delivered order(s)");

            if (amountNeeded > 0)
                Console.WriteLine($"  OR spend R{amountNeeded:F2} more");
        }

        #endregion
    }
}