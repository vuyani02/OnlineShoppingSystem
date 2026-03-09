# 🛒 OnlineShoppingSystem

## What is OnlineShoppingSystem?

OnlineShoppingSystem is a console-based backend shopping application built in **C#** as part of a graduate development programme. It simulates the backend logic of a real e-commerce platform where customers can browse products, manage shopping carts, place orders, and process payments, while administrators manage products, inventory, and orders.

The system demonstrates strong use of **object-oriented programming**, **class inheritance**, **polymorphism**, **interfaces**, **LINQ querying**, **exception handling**, and **clean architecture** within a console-based environment. All data is persisted using **JSON files**.

---

## 📄 Menus

| Menu | Description |
|---|---|
| Main Menu | Register, login or exit the system |
| Customer Menu | Browse products, manage cart, checkout, track orders and review products |
| Administrator Menu | Manage products, restock inventory, update orders and generate sales reports |

---

## 🧩 Features & Functional Requirements

### 1. Authentication
- User registration as Customer or Administrator
- Login with email and password validation
- Role-based menu routing — Customer or Administrator
- Input validation and exception handling on all auth operations

### 2. Customer Features
- Browse all available products sorted by category
- Search products by name — case insensitive
- Add products to cart with quantity selection
- View and update shopping cart
- Checkout with wallet payment
- View wallet balance and top up funds
- View full order history
- Track individual orders by ID
- Review and rate purchased products

### 3. Administrator Features
- Add new products to the catalog
- Update existing product details
- Delete products from the catalog
- Restock products with low inventory
- View all products and orders
- Update order status through the full lifecycle
- View low stock product alerts
- Generate sales reports with LINQ analytics

### 4. Payment System
- Simulated wallet-based payment
- Balance validation before checkout
- Automatic balance deduction on successful order

### 5. Inventory Management
- Stock tracking per product
- Automatic stock reduction on order placement
- Low stock threshold alerts
- Admin restock functionality

### 6. Order Management
- Full order lifecycle: Pending → Processing → Shipped → Delivered
- Order cancellation for Pending and Processing orders
- Order tracking by ID
- Order history per customer

### 7. Exception Handling
- `InsufficientBalanceException` — wallet too low
- `OutOfStockException` — product stock insufficient
- `InvalidLoginException` — wrong credentials
- `DuplicateEmailException` — email already registered
- `ProductNotFoundException` — product not found
- `OrderNotFoundException` — order not found
- `EmptyCartException` — checkout with empty cart
- `InvalidOrderStatusException` — invalid status transition

### 8. Data Persistence
- All data saved and loaded from JSON files
- Automatic `data/` folder creation on first save
- Separate JSON files per domain

---

## ⚙️ How to Run Locally

```bash
# 1. Clone the repository
git clone https://github.com/vuyani02/OnlineShoppingSystem.git

# 2. Navigate to the project folder
cd OnlineShoppingSystem

# 3. Run the application
dotnet run
```

---

## 🔑 Demo Credentials

| Role | Email | Password |
|---|---|---|
| Administrator | a@gmail.com | 12345 |
| Customer | v@gmail.com | 12345 |

---

## 🧪 Tech Stack

| Technology | Purpose |
|---|---|
| C# | Primary programming language |
| .NET 8 | Runtime framework |
| System.Text.Json | JSON serialization and deserialization |
| LINQ | Data querying and analytics |
| Visual Studio 2022 | IDE |

---

## 📁 Project Structure

```
OnlineShoppingSystem/
│
├── Models/                          # Domain classes
│   ├── User.cs                      # Base user class
│   ├── Customer.cs                  # Inherits from User
│   ├── Administrator.cs             # Inherits from User
│   ├── Product.cs
│   ├── Cart.cs
│   ├── CartItem.cs
│   ├── Order.cs
│   ├── OrderItem.cs
│   └── Review.cs
│
├── Repositories/                    # Data access layer
│   ├── IRepository.cs               # Generic repository interface
│   ├── UserRepository.cs
│   ├── ProductRepository.cs
│   └── OrderRepository.cs
│
├── Services/                        # Business logic layer
│   ├── UserService.cs
│   ├── ProductService.cs
│   └── OrderService.cs
│
├── Factories/                       # User creation logic
│   └── UserFactory.cs
│
├── Strategies/                      # Payment processing
│   ├── IPaymentStrategy.cs
│   └── WalletPayment.cs
│
├── Exceptions/                      # Custom exceptions
│   ├── InsufficientBalanceException.cs
│   ├── OutOfStockException.cs
│   ├── InvalidLoginException.cs
│   ├── DuplicateEmailException.cs
│   ├── ProductNotFoundException.cs
│   ├── OrderNotFoundException.cs
│   ├── EmptyCartException.cs
│   └── InvalidOrderStatusException.cs
│
├── data/                            # JSON storage (auto-generated)
│   ├── users.json
│   ├── products.json
│   └── orders.json
│
├── DataStore.cs                     # Shared data instance
└── Program.cs                       # Console menus and entry point
```

---

## 🏗️ Architecture

```
Program.cs          ← Console menus and user interaction
        ↓
Services            ← Business logic (register, checkout, reports)
        ↓
Repositories        ← Data access (save, load, find)
        ↓
data/*.json         ← JSON file storage
```

---

## 🎯 OOP Concepts Demonstrated

| Concept | Implementation |
|---|---|
| **Inheritance** | `Customer` and `Administrator` inherit from `User` |
| **Polymorphism** | `DisplayInfo()` overridden in each user subclass |
| **Interfaces** | `IRepository<T>` implemented by all repositories |
| **Encapsulation** | Private fields with controlled public access |
| **Abstraction** | Services hide data access details from the console menu |

---

## 👨‍💻 Developer

| Field | Detail |
|---|---|
| Project Name | OnlineShoppingSystem |
| Developer | Vuyani Matshungwana |
| Type | Console Application |
| Language | C# / .NET 8 |
| Purpose | Graduate Training Project — Submission 1 |

---

## 📄 License

This project was developed as a graduate training project. All rights reserved.