using System.Text.Json;
using OnlineShoppingSystem.Exceptions;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Repositories
{
    /// <summary>
    /// Handles all data operations for users.
    /// Reads and writes to users.json.
    /// </summary>
    public class UserRepository : IRepository<User>
    {
        #region Constants

        private const string FilePath = "data/users.json";

        #endregion

        #region Properties

        private List<User> _users;

        #endregion

        #region Constructor

        public UserRepository()
        {
            _users = LoadFromJson();
        }

        #endregion

        #region IRepository Methods

        /// <summary>
        /// Adds a new user to the system.
        /// Throws DuplicateEmailException if email already exists.
        /// </summary>
        public void Add(User user)
        {
            if (EmailExists(user.Email))
                throw new DuplicateEmailException(user.Email);

            _users.Add(user);
            SaveToJson();
        }

        /// <summary>
        /// Removes a user from the system.
        /// </summary>
        public void Remove(User user)
        {
            _users.Remove(user);
            SaveToJson();
        }

        /// <summary>
        /// Updates an existing user's details.
        /// </summary>
        public void Update(User updatedUser)
        {
            User existing = GetById(updatedUser.UserID);
            _users.Remove(existing);
            _users.Add(updatedUser);
            SaveToJson();
        }

        /// <summary>
        /// Returns a user by their ID.
        /// </summary>
        public User GetById(int id)
        {
            return _users.FirstOrDefault(u => u.UserID == id);
        }

        /// <summary>
        /// Returns all users in the system.
        /// </summary>
        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        #endregion

        #region Extra Methods

        /// <summary>
        /// Finds a user by email and password for login.
        /// Throws InvalidLoginException if credentials are incorrect.
        /// </summary>
        public User Login(string email, string password)
        {
            User user = _users.FirstOrDefault(u => u.Email == email);

            if (user == null || !user.ValidatePassword(password))
                throw new InvalidLoginException();

            return user;
        }

        /// <summary>
        /// Returns the next available user ID.
        /// </summary>
        public int GetNextID()
        {
            return _users.Count == 0 ? 1 : _users.Max(u => u.UserID) + 1;
        }

        /// <summary>
        /// Checks if an email address already exists in the system.
        /// </summary>
        public bool EmailExists(string email)
        {
            return _users.Any(u => u.Email == email);
        }

        #endregion

        #region JSON Methods

        /// <summary>
        /// Saves the current list of users to the JSON file.
        /// </summary>
        public void SaveToJson()
        {
            EnsureDataFolderExists();
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(_users, options);
            File.WriteAllText(FilePath, json);
        }

        /// <summary>
        /// Loads users from the JSON file.
        /// Returns empty list if file does not exist.
        /// </summary>
        private List<User> LoadFromJson()
        {
            if (!File.Exists(FilePath))
                return new List<User>();

            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Creates the data folder if it does not already exist.
        /// </summary>
        private void EnsureDataFolderExists()
        {
            if (!Directory.Exists("data"))
                Directory.CreateDirectory("data");
        }

        #endregion
    }
}