using OnlineShoppingSystem.Exceptions;
using OnlineShoppingSystem.Models;
using OnlineShoppingSystem.Repositories;
using OnlineShoppingSystem.Models;

namespace OnlineShoppingSystem.Services
{
    /// <summary>
    /// Handles all product related business logic.
    /// Manages browsing, searching, adding and updating products.
    /// </summary>
    public class ProductService
    {
        #region Properties

        private readonly ProductRepository _productRepo;

        #endregion

        #region Constructor

        public ProductService()
        {
            _productRepo = DataStore.Instance.ProductRepository;
        }

        #endregion

        #region Customer Methods

        /// <summary>
        /// Returns all available products sorted by category.
        /// </summary>
        public IEnumerable<Product> BrowseProducts()
        {
            IEnumerable<Product> products = _productRepo.GetAvailableProducts();

            if (!products.Any())
                throw new InvalidOperationException("No products available at the moment.");

            return products;
        }

        /// <summary>
        /// Searches for products by name — case insensitive.
        /// </summary>
        public IEnumerable<Product> SearchProducts(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Search term cannot be empty.");

            IEnumerable<Product> results = _productRepo.SearchByName(name);

            if (!results.Any())
                throw new InvalidOperationException($"No products found matching '{name}'.");

            return results;
        }

        /// <summary>
        /// Returns a single product by ID.
        /// </summary>
        public Product GetProduct(int productID)
        {
            return _productRepo.GetById(productID);
        }

        #endregion

        #region Admin Methods

        /// <summary>
        /// Adds a new product to the catalog.
        /// </summary>
        public void AddProduct(string name, string description, string category, double price, int stock)
        {
            ValidateProductInputs(name, description, category, price, stock);

            int productID = _productRepo.GetNextID();
            Product product = new Product(productID, name, description, category, price, stock);

            _productRepo.Add(product);
            Console.WriteLine($"✓ Product '{name}' added successfully!");
        }

        /// <summary>
        /// Updates an existing product's details.
        /// </summary>
        public void UpdateProduct(int productID, string name, string description, string category, double price)
        {
            Product product = _productRepo.GetById(productID);
            product.Name = name;
            product.Description = description;
            product.Category = category;
            product.Price = price;

            _productRepo.Update(product);
            Console.WriteLine($"✓ Product '{name}' updated successfully!");
        }

        /// <summary>
        /// Deletes a product from the catalog.
        /// </summary>
        public void DeleteProduct(int productID)
        {
            Product product = _productRepo.GetById(productID);
            _productRepo.Remove(product);
            Console.WriteLine($"✓ Product '{product.Name}' deleted successfully!");
        }

        /// <summary>
        /// Restocks a product by increasing its stock level.
        /// </summary>
        public void RestockProduct(int productID, int quantity)
        {
            Product product = _productRepo.GetById(productID);
            product.RestockProduct(quantity);
            _productRepo.Update(product);
        }

        /// <summary>
        /// Returns all products with low stock for admin review.
        /// </summary>
        public IEnumerable<Product> GetLowStockProducts()
        {
            return _productRepo.GetLowStockProducts();
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Validates all product input fields before saving.
        /// </summary>
        private void ValidateProductInputs(string name, string description, string category, double price, int stock)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be empty.");

            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Category cannot be empty.");

            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.");

            if (stock < 0)
                throw new ArgumentException("Stock cannot be negative.");
        }

        #endregion
    }
}