using OnlineShoppingSystemSystem.Models;

namespace OnlineShoppingSystemSystem.Models
{
    /// <summary>
    /// Represents an administrator in the Online Shopping system.
    /// Inherits from User and adds product and order management functionality.
    /// </summary>
    public class Administrator : User
    {
        #region Properties

        public DateTime LastLoginDate { get; private set; }
        public int TotalActionsPerformed { get; private set; }

        #endregion

        #region Constructor

        public Administrator(int userID, string firstName, string lastName, string email, string password)
            : base(userID, firstName, lastName, email, password, "Administrator")
        {
            LastLoginDate = DateTime.Now;
            TotalActionsPerformed = 0;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Logs an admin action and increments the action counter.
        /// </summary>
        public void LogAction(string action)
        {
            TotalActionsPerformed++;
            Console.WriteLine($"[ADMIN LOG] {FullName}: {action}");
        }

        /// <summary>
        /// Updates the last login date to current time.
        /// </summary>
        public void UpdateLastLogin()
        {
            LastLoginDate = DateTime.Now;
        }

        /// <summary>
        /// Displays administrator info including activity stats.
        /// </summary>
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Last Login:    {LastLoginDate:dd MMM yyyy HH:mm}");
            Console.WriteLine($"Total Actions: {TotalActionsPerformed}");
        }

        #endregion
    }
}