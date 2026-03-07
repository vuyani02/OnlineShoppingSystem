using OnlineShoppingSystem.Exceptions;
using OnlineShoppingSystem.Factories;
using OnlineShoppingSystem.Models;
using OnlineShoppingSystem.Repositories;

namespace OnlineShoppingSystem.Services
{
    /// <summary>
    /// Handles all user related business logic.
    /// Manages registration, login and user retrieval.
    /// </summary>
    public class UserService
    {
        #region Properties

        private readonly UserRepository _userRepo;

        #endregion

        #region Constructor

        public UserService()
        {
            _userRepo = DataStore.Instance.UserRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Registers a new user using the UserFactory to create the correct type.
        /// Throws DuplicateEmailException if the email is already registered.
        /// </summary>
        public void Register(string role, string firstName, string lastName, string email, string password)
        {
            ValidateRegistrationInputs(firstName, lastName, email, password);

            int userID = _userRepo.GetNextID();
            User user = UserFactory.CreateUser(role, userID, firstName, lastName, email, password);

            _userRepo.Add(user);
            Console.WriteLine($"✓ Account created successfully! Welcome {user.FullName}.");
        }

        /// <summary>
        /// Logs in a user and returns their account.
        /// Throws InvalidLoginException if credentials are incorrect.
        /// </summary>
        public User Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new InvalidLoginException();

            return _userRepo.Login(email, password);
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Validates all registration input fields are not empty.
        /// </summary>
        private void ValidateRegistrationInputs(string firstName, string lastName, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be empty.");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be empty.");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.");
        }

        #endregion
    }
}