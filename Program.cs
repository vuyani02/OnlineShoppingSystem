using OnlineShoppingSystem.Exceptions;
using OnlineShoppingSystem.Models;
using OnlineShoppingSystem.Services;
using System;
using System.Data;
using System.Runtime.Intrinsics.X86;

namespace OnlineShoppingSystem
{
    /// <summary>
    /// Entry point of the Online Shopping System.
    /// Handles all console menus and user interaction.
    /// </summary>
    internal class Program
    {
        #region Properties

        private static UserService _userService = new UserService();
        private static ProductService _productService = new ProductService();
        private static OrderService _orderService = new OrderService();
        private static Customer _loggedInCustomer = null;
        private static Administrator _loggedInAdmin = null;

        #endregion

        #region Entry Point

        static void Main(string[] args)
        {
            ShowMainMenu();
        }

        #endregion

        #region Main Menu

        /// <summary>
        /// Displays the main menu and routes to login or register.
        /// </summary>
        private static void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════════════════╗");
                Console.WriteLine("║        WELCOME TO ONLINE SHOPPING SYSTEM     ║");
                Console.WriteLine("╠══════════════════════════════════════════════╣");
                Console.WriteLine("║  1. Register                                 ║");
                Console.WriteLine("║  2. Login                                    ║");
                Console.WriteLine("║  3. Exit                                     ║");
                Console.WriteLine("╚══════════════════════════════════════════════╝");
                Console.Write("\nEnter choice: ");

                switch (Console.ReadLine().Trim())
                {
                    case "1": Register(); break;
                    case "2": Login(); break;
                    case "3":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        ShowError("Invalid choice. Please enter 1, 2 or 3.");
                        break;
                }
            }
        }

        #endregion

        #region Auth Menus

        /// <summary>
        /// Handles user registration input and calls UserService.
        /// </summary>
        private static void Register()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║                  REGISTER                    ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝\n");

            Console.WriteLine("Select role:");
            Console.WriteLine("1. Customer");
            Console.WriteLine("2. Administrator");
            Console.Write("Enter choice: ");

            string role = Console.ReadLine().Trim() switch
            {
                "1" => "Customer",
                "2" => "Administrator",
                _ => null
            };

            if (role == null)
            {
                ShowError("Invalid role selected.");
                return;
            }

            try
            {
                Console.Write("First Name:  ");
                string firstName = Console.ReadLine().Trim();

                Console.Write("Last Name:   ");
                string lastName = Console.ReadLine().Trim();

                Console.Write("Email:       ");
                string email = Console.ReadLine().Trim();

                Console.Write("Password:    ");
                string password = Console.ReadLine().Trim();

                _userService.Register(role, firstName, lastName, email, password);
                PressAnyKey();
            }
            catch (DuplicateEmailException ex) { ShowError(ex.Message); }
            catch (ArgumentException ex) { ShowError(ex.Message); }
            catch (Exception ex) { ShowError($"Unexpected error: {ex.Message}"); }
        }

        /// <summary>
        /// Handles login input and routes to the correct menu based on role.
        /// </summary>
        private static void Login()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║                    LOGIN                     ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝\n");

            try
            {
                Console.Write("Email:    ");
                string email = Console.ReadLine().Trim();

                Console.Write("Password: ");
                string password = Console.ReadLine().Trim();

                // Get the user from repository
                User user = _userService.Login(email, password);

                Console.WriteLine($"\n✓ Welcome back, {user.FullName}!");
                PressAnyKey();

                // Check role and route to correct menu
                if (user.Role == "Customer")
                {
                    _loggedInCustomer = new Customer(user.UserID, user.FirstName, user.LastName, user.Email, user.Password);
                    ShowCustomerMenu(_loggedInCustomer);
                }
                else if (user.Role == "Administrator")
                {
                    _loggedInAdmin = new Administrator(user.UserID, user.FirstName, user.LastName, user.Email, user.Password);
                    ShowAdminMenu(_loggedInAdmin);
                }
            }
            catch (InvalidLoginException ex) { ShowError(ex.Message); }
            catch (Exception ex) { ShowError($"Unexpected error: {ex.Message}"); }
        }

        #endregion

        #region Customer Menu

        /// <summary>
        /// Displays the customer menu and handles all customer operations.
        /// </summary>
        private static void ShowCustomerMenu(Customer customer)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════════════════╗");
                Console.WriteLine($"║  Welcome, {customer.FullName,-35}║");
                Console.WriteLine($"║  $ Wallet: R{customer.WalletBalance,-33:F2}║");
                Console.WriteLine("╠══════════════════════════════════════════════╣");
                Console.WriteLine("║  1.  Browse Products                         ║");
                Console.WriteLine("║  2.  Search Products                         ║");
                Console.WriteLine("║  3.  Add Product to Cart                     ║");
                Console.WriteLine("║  4.  View Cart                               ║");
                Console.WriteLine("║  5.  Update Cart                             ║");
                Console.WriteLine("║  6.  Checkout                                ║");
                Console.WriteLine("║  7.  View Wallet Balance                     ║");
                Console.WriteLine("║  8.  Add Wallet Funds                        ║");
                Console.WriteLine("║  9.  View Order History                      ║");
                Console.WriteLine("║  10. Track Order                             ║");
                Console.WriteLine("║  11. Review a Product                        ║");
                Console.WriteLine("║  12. View Discount Status                    ║");
                Console.WriteLine("║  13. Logout                                  ║");
                Console.WriteLine("╚══════════════════════════════════════════════╝");
                Console.Write("\nEnter choice: ");

                switch (Console.ReadLine().Trim())
                {
                    case "1": BrowseProducts(); PressAnyKey(); break;
                    case "2": SearchProducts(); break;
                    case "3": AddToCart(customer); break;
                    case "4":
                        Console.Clear();
                        customer.Cart.DisplayCart();
                        PressAnyKey(); break;
                    case "5": UpdateCart(customer); break;
                    case "6": Checkout(customer); break;
                    case "7": ViewWallet(customer); break;
                    case "8": AddWalletFunds(customer); break;
                    case "9": ViewOrderHistory(customer); break;
                    case "10": TrackOrder(); break;
                    case "11": ReviewProduct(customer); break;
                    case "12": ViewDiscountStatus(customer); break;
                    case "13":
                        _loggedInCustomer = null;
                        Console.WriteLine("[OK] Logged out successfully.");
                        PressAnyKey();
                        return;
                    default:
                        ShowError("Invalid choice. Please enter a number between 1 and 13.");
                        break;
                }
            }
        }

        #endregion

        #region Customer Operations

        /// <summary>
        /// Displays all available products in the catalog.
        /// </summary>
        private static void BrowseProducts()
        {
            Console.Clear();
            Console.WriteLine("=== PRODUCT CATALOG ===\n");

            try
            {
                var products = _productService.BrowseProducts();
                Console.WriteLine($"{"ID",-5} {"Name",-25} {"Category",-15} {"Price",-12} {"Stock",-8} {"Rating"}");
                Console.WriteLine(new string('─', 77));

                foreach (var product in products)
                    product.DisplaySummary();
            }
            catch (InvalidOperationException ex) { ShowError(ex.Message); }
        }

        /// <summary>
        /// Searches for products by name.
        /// </summary>
        private static void SearchProducts()
        {
            Console.Clear();
            Console.WriteLine("=== SEARCH PRODUCTS ===\n");
            Console.Write("Enter product name to search: ");

            string name = Console.ReadLine();

            try
            {
                var results = _productService.SearchProducts(name);
                Console.WriteLine($"\nResults for '{name}':\n");

                foreach (var product in results)
                    product.DisplaySummary();
            }
            catch (ArgumentException ex) { ShowError(ex.Message); }
            catch (InvalidOperationException ex) { ShowError(ex.Message); }

            PressAnyKey();
        }

        /// <summary>
        /// Adds a product to the customer's cart.
        /// </summary>
        private static void AddToCart(Customer customer)
        {
            Console.Clear();
            Console.WriteLine("=== ADD TO CART ===\n");

            try
            {
                BrowseProducts();
                Console.WriteLine();

                Console.Write("Enter Product ID: ");
                if (!int.TryParse(Console.ReadLine().Trim(), out int productID))
                {
                    ShowError("Invalid product ID.");
                    return;
                }

                Console.Write("Enter Quantity:   ");
                if (!int.TryParse(Console.ReadLine().Trim(), out int quantity))
                {
                    ShowError("Invalid quantity.");
                    return;
                }

                var product = _productService.GetProduct(productID);
                customer.Cart.AddItem(product, quantity);
                PressAnyKey();
            }
            catch (ProductNotFoundException ex) { ShowError(ex.Message); }
            catch (OutOfStockException ex) { ShowError(ex.Message); }
            catch (ArgumentException ex) { ShowError(ex.Message); }
        }

        /// <summary>
        /// Updates the quantity of an item in the cart or removes it.
        /// </summary>
        private static void UpdateCart(Customer customer)
        {
            Console.Clear();
            Console.WriteLine("=== UPDATE CART ===\n");

            customer.Cart.DisplayCart();

            if (customer.Cart.IsEmpty)
            {
                PressAnyKey();
                return;
            }

            try
            {
                Console.Write("\nEnter Product ID to update: ");
                if (!int.TryParse(Console.ReadLine().Trim(), out int productID))
                {
                    ShowError("Invalid product ID.");
                    return;
                }

                Console.Write("Enter new quantity (0 to remove): ");
                if (!int.TryParse(Console.ReadLine().Trim(), out int quantity))
                {
                    ShowError("Invalid quantity.");
                    return;
                }

                customer.Cart.UpdateQuantity(productID, quantity);
                PressAnyKey();
            }
            catch (InvalidOperationException ex) { ShowError(ex.Message); }
            catch (ArgumentException ex) { ShowError(ex.Message); }
        }

        /// <summary>
        /// Processes checkout for the customer.
        /// </summary>
        private static void Checkout(Customer customer)
        {
            Console.Clear();
            Console.WriteLine("=== CHECKOUT ===\n");

            customer.Cart.DisplayCart();

            if (customer.Cart.IsEmpty)
            {
                PressAnyKey();
                return;
            }

            Console.Write($"\nTotal: R{customer.Cart.TotalPrice:F2} — Confirm? (y/n): ");

            if (Console.ReadLine().Trim()?.ToLower() != "y")
            {
                Console.WriteLine("Checkout cancelled.");
                PressAnyKey();
                return;
            }

            try
            {
                _orderService.Checkout(customer);
                PressAnyKey();
            }
            catch (EmptyCartException ex) { ShowError(ex.Message); }
            catch (InsufficientBalanceException ex)
            {
                ShowError(ex.Message);
                Console.WriteLine($"  Top up R{ex.RequiredAmount - ex.AvailableAmount:F2} to proceed.");
                PressAnyKey();
            }
            catch (OutOfStockException ex) { ShowError(ex.Message); }
            catch (Exception ex) { ShowError($"Unexpected error: {ex.Message}"); }
        }

        /// <summary>
        /// Displays the customer's wallet balance.
        /// </summary>
        private static void ViewWallet(Customer customer)
        {
            Console.Clear();
            Console.WriteLine("=== WALLET BALANCE ===\n");
            Console.WriteLine($"  Current Balance: R{customer.WalletBalance:F2}");
            PressAnyKey();
        }

        /// <summary>
        /// Adds funds to the customer's wallet.
        /// </summary>
        private static void AddWalletFunds(Customer customer)
        {
            Console.Clear();
            Console.WriteLine("=== ADD WALLET FUNDS ===\n");
            Console.Write("Enter amount to add: R");

            try
            {
                if (!double.TryParse(Console.ReadLine().Trim(), out double amount))
                {
                    ShowError("Invalid amount.");
                    return;
                }

                customer.TopUpWallet(amount);
                PressAnyKey();
            }
            catch (ArgumentException ex) { ShowError(ex.Message); }
        }

        /// <summary>
        /// Displays the customer's full order history.
        /// </summary>
        private static void ViewOrderHistory(Customer customer)
        {
            Console.Clear();
            Console.WriteLine("=== ORDER HISTORY ===\n");

            try
            {
                Console.WriteLine($"{"ID",-5} {"Status",-11} {"TotalItems",-15} {"TotalAmount",-15} {"Date"}");
                Console.WriteLine(new string('─', 61));

                var orders = _orderService.GetOrderHistory(customer.UserID);
                foreach (var order in orders)
                {
                    order.DisplaySummary();
                }
            }
            catch (InvalidOperationException ex) { ShowError(ex.Message); }

            PressAnyKey();
        }

        /// <summary>
        /// Tracks a specific order by ID.
        /// </summary>
        private static void TrackOrder()
        {
            Console.Clear();
            Console.WriteLine("=== TRACK ORDER ===\n");
            Console.Write("Enter Order ID: ");

            try
            {
                if (!int.TryParse(Console.ReadLine().Trim(), out int orderID))
                {
                    ShowError("Invalid order ID.");
                    return;
                }

                Order order = _orderService.TrackOrder(orderID);
                order.DisplayInfo();
            }
            catch (OrderNotFoundException ex) { ShowError(ex.Message); }

            PressAnyKey();
        }

        /// <summary>
        /// Allows a customer to review a product they have purchased.
        /// </summary>
        private static void ReviewProduct(Customer customer)
        {
            Console.Clear();
            Console.WriteLine("=== REVIEW A PRODUCT ===\n");
            Console.Write("Enter Product ID to review: ");

            try
            {
                if (!int.TryParse(Console.ReadLine().Trim(), out int productID))
                {
                    ShowError("Invalid product ID.");
                    return;
                }

                Product product = _productService.GetProduct(productID);

                Console.Write("Rating (1-5):  ");
                if (!double.TryParse(Console.ReadLine().Trim(), out double rating))
                {
                    ShowError("Invalid rating.");
                    return;
                }

                Console.Write("Comment:       ");
                string comment = Console.ReadLine().Trim();

                int reviewID = product.Reviews.Count + 1;
                Review review = new Review(reviewID, productID, customer.UserID, customer.FullName, rating, comment);
                product.AddReview(review);

                DataStore.Instance.ProductRepository.Update(product);
                Console.WriteLine("✓ Review submitted successfully!");
                PressAnyKey();
            }
            catch (ProductNotFoundException ex) { ShowError(ex.Message); }
            catch (ArgumentException ex) { ShowError(ex.Message); }
        }

        /// <summary>
        /// Displays the customer's current discount tier and progress.
        /// </summary>
        private static void ViewDiscountStatus(Customer customer)
        {
            Console.Clear();
            _orderService.GetDiscountService().DisplayCustomerDiscount(customer.UserID);
            _orderService.GetDiscountService().DisplayAllTiers();
            PressAnyKey();
        }

        #endregion

        #region Admin Menu

        /// <summary>
        /// Displays the admin menu and handles all admin operations.
        /// </summary>
        private static void ShowAdminMenu(Administrator admin)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════════════════╗");
                Console.WriteLine($"║  Admin: {admin.FullName,-37}║");
                Console.WriteLine("╠══════════════════════════════════════════════╣");
                Console.WriteLine("║  1.  Add Product                             ║");
                Console.WriteLine("║  2.  Update Product                          ║");
                Console.WriteLine("║  3.  Delete Product                          ║");
                Console.WriteLine("║  4.  Restock Product                         ║");
                Console.WriteLine("║  5.  View All Products                       ║");
                Console.WriteLine("║  6.  View All Orders                         ║");
                Console.WriteLine("║  7.  Update Order Status                     ║");
                Console.WriteLine("║  8.  View Low Stock Products                 ║");
                Console.WriteLine("║  9.  Generate Sales Report                   ║");
                Console.WriteLine("║  10. Logout                                  ║");
                Console.WriteLine("╚══════════════════════════════════════════════╝");
                Console.Write("\nEnter choice: ");

                switch (Console.ReadLine().Trim())
                {
                    case "1": AddProduct(admin); break;
                    case "2": UpdateProduct(admin); break;
                    case "3": DeleteProduct(admin); break;
                    case "4": RestockProduct(admin); break;
                    case "5": ViewAllProducts(); PressAnyKey(); break;
                    case "6": ViewAllOrders(); PressAnyKey(); break;
                    case "7": UpdateOrderStatus(admin); break;
                    case "8": ViewLowStockProducts(); PressAnyKey(); break;
                    case "9":
                        Console.Clear();
                        _orderService.GenerateSalesReport();
                        PressAnyKey(); break;
                    case "10":
                        _loggedInAdmin = null;
                        Console.WriteLine("✓ Logged out successfully.");
                        PressAnyKey();
                        return;
                    default:
                        ShowError("Invalid choice. Please enter a number between 1 and 10.");
                        break;
                }
            }
        }

        #endregion

        #region Admin Operations

        /// <summary>
        /// Handles adding a new product to the catalog.
        /// </summary>
        private static void AddProduct(Administrator admin)
        {
            Console.Clear();
            Console.WriteLine("=== ADD PRODUCT ===\n");

            try
            {
                Console.Write("Name:        ");
                string name = Console.ReadLine().Trim();

                Console.Write("Description: ");
                string description = Console.ReadLine().Trim();

                Console.Write("Category:    ");
                string category = Console.ReadLine().Trim();

                Console.Write("Price: R");
                if (!double.TryParse(Console.ReadLine().Trim(), out double price))
                {
                    ShowError("Invalid price.");
                    return;
                }

                Console.Write("Stock:       ");
                if (!int.TryParse(Console.ReadLine().Trim(), out int stock))
                {
                    ShowError("Invalid stock.");
                    return;
                }

                _productService.AddProduct(name, description, category, price, stock);
                admin.LogAction($"Added product: {name}");
                PressAnyKey();
            }
            catch (ArgumentException ex) { ShowError(ex.Message); }
        }

        /// <summary>
        /// Handles updating an existing product.
        /// </summary>
        private static void UpdateProduct(Administrator admin)
        {
            Console.Clear();
            Console.WriteLine("=== UPDATE PRODUCT ===\n");

            try
            {
                ViewAllProducts();

                Console.WriteLine();

                Console.Write("Enter Product ID to update: ");
                if (!int.TryParse(Console.ReadLine().Trim(), out int productID))
                {
                    ShowError("Invalid product ID.");
                    return;
                }

                Console.Write("New Name:        ");
                string name = Console.ReadLine().Trim();

                Console.Write("New Description: ");
                string description = Console.ReadLine().Trim();

                Console.Write("New Category:    ");
                string category = Console.ReadLine().Trim();

                Console.Write("New Price: R");
                if (!double.TryParse(Console.ReadLine().Trim(), out double price))
                {
                    ShowError("Invalid price.");
                    return;
                }

                _productService.UpdateProduct(productID, name, description, category, price);
                admin.LogAction($"Updated product ID: {productID}");
                PressAnyKey();
            }
            catch (ProductNotFoundException ex) { ShowError(ex.Message); }
            catch (ArgumentException ex) { ShowError(ex.Message); }
        }

        /// <summary>
        /// Handles deleting a product from the catalog.
        /// </summary>
        private static void DeleteProduct(Administrator admin)
        {
            Console.Clear();
            Console.WriteLine("=== DELETE PRODUCT ===\n");

            try
            {
                ViewAllProducts();

                Console.Write("Enter Product ID to delete: ");
                if (!int.TryParse(Console.ReadLine().Trim(), out int productID))
                {
                    ShowError("Invalid product ID.");
                    return;
                }

                Console.Write("Are you sure? (y/n): ");
                if (Console.ReadLine().Trim()?.ToLower() != "y")
                {
                    Console.WriteLine("Delete cancelled.");
                    PressAnyKey();
                    return;
                }

                _productService.DeleteProduct(productID);
                admin.LogAction($"Deleted product ID: {productID}");
                PressAnyKey();
            }
            catch (ProductNotFoundException ex) { ShowError(ex.Message); }
        }

        /// <summary>
        /// Handles restocking a product.
        /// </summary>
        private static void RestockProduct(Administrator admin)
        {
            Console.Clear();
            Console.WriteLine("=== RESTOCK PRODUCT ===\n");

            try
            {
                ViewLowStockProducts();

                Console.WriteLine();

                Console.Write("Enter Product ID to restock: ");
                if (!int.TryParse(Console.ReadLine().Trim(), out int productID))
                {
                    ShowError("Invalid product ID.");
                    return;
                }

                Console.Write("Enter quantity to add: ");
                if (!int.TryParse(Console.ReadLine().Trim(), out int quantity))
                {
                    ShowError("Invalid quantity.");
                    return;
                }

                _productService.RestockProduct(productID, quantity);
                admin.LogAction($"Restocked product ID: {productID} by {quantity}");
                PressAnyKey();
            }
            catch (ProductNotFoundException ex) { ShowError(ex.Message); }
            catch (ArgumentException ex) { ShowError(ex.Message); }
        }

        /// <summary>
        /// Displays all products in the catalog.
        /// </summary>
        private static void ViewAllProducts()
        {
            Console.Clear();
            Console.WriteLine("=== ALL PRODUCTS ===\n");

            var products = _productService.BrowseProducts();
            Console.WriteLine($"{"ID",-5} {"Name",-25} {"Category",-15} {"Price",-11} {"Stock",-8} {"Rating"}");
            Console.WriteLine(new string('─', 77));

            foreach (var product in products)
                product.DisplaySummary();
        }

        /// <summary>
        /// Displays all orders in the system.
        /// </summary>
        private static void ViewAllOrders()
        {
            Console.Clear();
            Console.WriteLine("=== ALL ORDERS ===\n");

            var orders = _orderService.GetAllOrders();

            if (!orders.Any())
            {
                Console.WriteLine("No orders found.");
                PressAnyKey();
                return;
            }

            Console.WriteLine($"{"ID",-5} {"Status",-11} {"TotalItems", -15} {"TotalAmount", -15} {"Date"}");
            Console.WriteLine(new string('─', 61));

            foreach (var order in orders)
                order.DisplaySummary();
        }

        /// <summary>
        /// Updates the status of an order.
        /// </summary>
        private static void UpdateOrderStatus(Administrator admin)
        {
            Console.Clear();
            Console.WriteLine("=== UPDATE ORDER STATUS ===\n");

            try
            {
                ViewAllOrders();

                Console.WriteLine();

                Console.Write("Enter Order ID: ");
                if (!int.TryParse(Console.ReadLine().Trim(), out int orderID))
                {
                    ShowError("Invalid order ID.");
                    return;
                }

                Console.WriteLine("Select new status:");
                Console.WriteLine("1. Processing");
                Console.WriteLine("2. Shipped");
                Console.WriteLine("3. Delivered");
                Console.WriteLine("4. Cancelled");
                Console.Write("Enter choice: ");

                string newStatus = Console.ReadLine().Trim() switch
                {
                    "1" => "Processing",
                    "2" => "Shipped",
                    "3" => "Delivered",
                    "4" => "Cancelled",
                    _ => null
                };

                if (newStatus == null)
                {
                    ShowError("Invalid status choice.");
                    return;
                }

                _orderService.UpdateOrderStatus(orderID, newStatus);
                admin.LogAction($"Updated order #{orderID} to {newStatus}");
                PressAnyKey();
            }
            catch (OrderNotFoundException ex) { ShowError(ex.Message); }
            catch (InvalidOrderStatusException ex) { ShowError(ex.Message); }
            catch (InvalidOperationException ex) { ShowError(ex.Message); }
        }

        /// <summary>
        /// Displays all products with low stock levels.
        /// </summary>
        private static void ViewLowStockProducts()
        {
            Console.Clear();
            Console.WriteLine("=== LOW STOCK PRODUCTS ===\n");

            var products = _productService.GetLowStockProducts();

            if (!products.Any())
            {
                Console.WriteLine("✓ No low stock products.");
                return;
            }

            Console.WriteLine($"{"ID",-5} {"Name",-25} {"Category",-15} {"Price",-12} {"Stock",-8} {"Rating"}");
            Console.WriteLine(new string('─', 77));

            foreach (var product in products)
                product.DisplaySummary();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Displays an error message in red and waits for user input.
        /// </summary>
        private static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n❌ {message}");
            Console.ResetColor();
            PressAnyKey();
        }

        /// <summary>
        /// Prompts the user to press any key to continue.
        /// </summary>
        private static void PressAnyKey()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        #endregion
    }
}