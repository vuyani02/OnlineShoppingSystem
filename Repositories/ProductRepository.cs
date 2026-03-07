using OnlineShoppingSystem.Exceptions;
using OnlineShoppingSystem.Models;
using OnlineShoppingSystemSystem.Models;
using System.Text.Json;

namespace OnlineShoppingSystem.Repositories
{
    /// <summary>
    /// Handles all data operations for products.
    /// Reads and writes to products.json.
    /// </summary>
    public class ProductRepository : IRepository<Product>
    {
        #region Constants

        private const string FilePath = "data/products.json";

        #endregion

        #region Properties

        private List<Product> _products;

        #endregion

        #region Constructor

        public ProductRepository()
        {
            _products = LoadFromJson();
        }

        #endregion

        #region IRepository Methods

        /// <summary>
        /// Adds a new product to the catalog.
        /// </summary>
        public void Add(Product product)
        {
            _products.Add(product);
            SaveToJson();
        }

        /// <summary>
        /// Removes a product from the catalog.
        /// </summary>
        public void Remove(Product product)
        {
            _products.Remove(product);
            SaveToJson();
        }

        /// <summary>
        /// Updates an existing product's details.
        /// </summary>
        public void Update(Product updatedProduct)
        {
            Product existing = GetById(updatedProduct.ProductID);
            _products.Remove(existing);
            _products.Add(updatedProduct);
            SaveToJson();
        }

        /// <summary>
        /// Returns a product by its ID.
        /// Throws ProductNotFoundException if not found.
        /// </summary>
        public Product GetById(int id)
        {
            Product product = _products.FirstOrDefault(p => p.ProductID == id);

            if (product == null)
                throw new ProductNotFoundException(id);

            return product;
        }

        /// <summary>
        /// Returns all products in the catalog.
        /// </summary>
        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        #endregion

        #region Extra Methods

        /// <summary>
        /// Searches products by name — case insensitive partial match.
        /// </summary>
        public IEnumerable<Product> SearchByName(string name)
        {
            return _products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Returns all products in a specific category.
        /// </summary>
        public IEnumerable<Product> GetByCategory(string category)
        {
            return _products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Returns all products with stock at or below the low stock threshold.
        /// </summary>
        public IEnumerable<Product> GetLowStockProducts()
        {
            return _products.Where(p => p.IsLowStock).OrderBy(p => p.Stock);
        }

        /// <summary>
        /// Returns all products currently available in stock.
        /// </summary>
        public IEnumerable<Product> GetAvailableProducts()
        {
            return _products.Where(p => p.IsAvailable).OrderBy(p => p.Category);
        }

        /// <summary>
        /// Returns the next available product ID.
        /// </summary>
        public int GetNextID()
        {
            return _products.Count == 0 ? 1 : _products.Max(p => p.ProductID) + 1;
        }

        #endregion

        #region JSON Methods

        /// <summary>
        /// Saves the current list of products to the JSON file.
        /// </summary>
        public void SaveToJson()
        {
            EnsureDataFolderExists();
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(_products, options);
            File.WriteAllText(FilePath, json);
        }

        /// <summary>
        /// Loads products from the JSON file.
        /// Returns empty list if file does not exist.
        /// </summary>
        private List<Product> LoadFromJson()
        {
            if (!File.Exists(FilePath))
                return new List<Product>();

            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
        }

        #endregion

        #region Private Helper Methods

        private void EnsureDataFolderExists()
        {
            if (!Directory.Exists("data"))
                Directory.CreateDirectory("data");
        }

        #endregion
    }
}