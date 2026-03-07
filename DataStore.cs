using OnlineShoppingSystem.Repositories;

namespace OnlineShoppingSystem
{
    /// <summary>
    /// Singleton class that holds all repositories.
    /// Ensures only one instance of each repository exists throughout the application.
    /// </summary>
    public class DataStore
    {
        #region Singleton

        private static DataStore _instance;

        public static DataStore Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DataStore();

                return _instance;
            }
        }

        private DataStore()
        {
            UserRepository = new UserRepository();
            ProductRepository = new ProductRepository();
            OrderRepository = new OrderRepository();
        }

        #endregion

        #region Properties

        public UserRepository UserRepository { get; private set; }
        public ProductRepository ProductRepository { get; private set; }
        public OrderRepository OrderRepository { get; private set; }

        #endregion
    }
}