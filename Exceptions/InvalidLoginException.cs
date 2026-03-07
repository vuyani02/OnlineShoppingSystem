namespace OnlineShoppingSystem.Exceptions
{
    /// <summary>
    /// Thrown when a user attempts to login with invalid credentials.
    /// </summary>
    public class InvalidLoginException : Exception
    {
        public InvalidLoginException()
            : base("Invalid email or password. Please try again.")
        {
        }
    }
}