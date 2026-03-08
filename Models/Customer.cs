using OnlineShoppingSystem.Exceptions;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Models
{
    /// <summary>
    /// Represents a customer in the Online Shopping system.
    /// Inherits from User and adds wallet, cart and order history functionality.
    /// </summary>
    public class Customer : User
    {
        #region Properties

        public double WalletBalance { get; private set; }
        public Cart Cart { get; private set; }
        public List<Order> OrderHistory { get; private set; }

        #endregion

        #region Constructor

        public Customer(int userID, string firstName, string lastName, string email, string password)
            : base(userID, firstName, lastName, email, password, "Customer")
        {
            WalletBalance = 0.00;
            Cart = new Cart(userID);
            OrderHistory = new List<Order>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds funds to the customer's wallet.
        /// </summary>
        public void TopUpWallet(double amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Top up amount must be greater than zero.");

            WalletBalance += amount;
            Console.WriteLine($"✓ Wallet topped up! New balance: R{WalletBalance:F2}");
        }

        /// <summary>
        /// Deducts the order total from the customer's wallet.
        /// Throws InsufficientBalanceException if balance is too low.
        /// </summary>
        public void DeductBalance(double amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");

            if (WalletBalance < amount)
                throw new InsufficientBalanceException(amount, WalletBalance);

            WalletBalance -= amount;
        }

        /// <summary>
        /// Adds a completed order to the customer's order history.
        /// </summary>
        public void AddOrderToHistory(Order order)
        {
            OrderHistory.Add(order);
        }

        /// <summary>
        /// Displays customer info including wallet balance and order count.
        /// </summary>
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Wallet Balance: R{WalletBalance:F2}");
            Console.WriteLine($"Total Orders:   {OrderHistory.Count}");
        }

        #endregion
    }
}