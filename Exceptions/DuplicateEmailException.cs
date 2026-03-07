namespace OnlineShoppingSystem.Exceptions
{
    /// <summary>
    /// Thrown when a user attempts to register with an email that already exists.
    /// </summary>
    public class DuplicateEmailException : Exception
    {
        public string Email { get; }

        public DuplicateEmailException(string email)
            : base($"An account with email '{email}' already exists.")
        {
            Email = email;
        }
    }
}