using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShoppingSystemSystem.Models
{
    /// <summary>
    /// Represents a base user in the Online Shopping system.
    /// Contains common properties and behaviour shared by all user types.
    /// </summary>
    public class User
    {
        #region Properties

        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }  // "Customer" or "Administrator"
        public string FullName => $"{FirstName} {LastName}";

        #endregion

        #region Constructor

        public User(int userID, string firstName, string lastName, string email, string password, string role)
        {
            UserID = userID;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Role = role;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates the provided password against the stored password.
        /// </summary>
        public bool ValidatePassword(string inputPassword)
        {
            return Password == inputPassword;
        }

        /// <summary>
        /// Displays the user's basic information to the console.
        /// Can be overridden by subclasses to show role-specific info.
        /// </summary>
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"ID:    {UserID}");
            Console.WriteLine($"Name:  {FullName}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"Role:  {Role}");
        }

        #endregion

    }

}
