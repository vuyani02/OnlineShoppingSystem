using OnlineShoppingSystem.Models;
using OnlineShoppingSystemSystem.Models;

namespace OnlineShoppingSystem.Factories
{
    /// <summary>
    /// Responsible for creating the correct user type based on role.
    /// Prevents services from needing to know how users are constructed.
    /// </summary>
    public static class UserFactory
    {
        /// <summary>
        /// Creates and returns a Customer or Administrator based on the role provided.
        /// Throws ArgumentException if the role is invalid.
        /// </summary>
        public static User CreateUser(string role, int userID, string firstName, string lastName, string email, string password)
        {
            return role switch
            {
                "Customer" => new Customer(userID, firstName, lastName, email, password),
                "Administrator" => new Administrator(userID, firstName, lastName, email, password),
                _ => throw new ArgumentException($"Invalid role: {role}. Must be Customer or Administrator.")
            };
        }
    }
}