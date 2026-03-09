# 🛒 OnlineShoppingSystem

## What is OnlineShoppingSystem?

OnlineShoppingSystem is a console-based backend shopping application built in **C#** as part of a graduate development programme. It simulates the backend logic of a real e-commerce platform where customers can browse products, manage shopping carts, place orders, and process payments, while administrators manage products, inventory, and orders.

The system demonstrates strong use of **object-oriented programming**, **class inheritance**, **polymorphism**, **interfaces**, **design patterns**, **LINQ querying**, **exception handling**, and **clean architecture** within a console-based environment. All data is persisted using **JSON files**.

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
- Checkout with wallet payment and automatic loyalty discount
- View wallet balance and top up funds
- View full order history
- Track individual orders by ID
- Review and rate purchased products
- View personal discount tier and progress to next tier

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
- Automatic loyalty discount deducted before payment
- Full breakdown shown at checkout — Subtotal, Discount, Total

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

### 7. Loyalty Discount System
Customers are automatically rewarded based on their order history. The system calculates the discount at checkout using the customer's delivered orders:

| Tier | Requirement | Discount |
|---|---|---|
| Bronze | 3+ delivered orders OR R2000+ spent | 5% off |
| Silver | 5+ delivered orders OR R5000+ spent | 10% off |
| Gold | 10+ delivered orders OR R10000+ spent | 15% off |

The discount is applied automatically at checkout and shown as a breakdown:
```
Subtotal:   R1000.00
Discount:   -10% (-R100.00)
Total:      R900.00
```

Customers can view their current tier and progress to the next tier from the customer menu.

### 8. Exception Handling
- `InsufficientBalanceException` — wallet too low
- `OutOfStockException` — product stock insufficient
- `InvalidLoginException` — wrong credentials
- `DuplicateEmailException` — email already registered
- `ProductNotFoundException` — product not found
- `OrderNotFoundException` — order not found
- `EmptyCartException` — checkout with empty cart
- `InvalidOrderStatusException` — invalid status transition

### 9. Data Persistence
- All data saved and loaded from JSON files
- Automatic `data/` folder creation on first save
- Separate JSON files per domain

---

## ⚙️ How to Run Locally

```bash
# 1. Clone the repository
git clone https://github.com/vuyani02/OnlineShoppingSystem.git
```

2. Open the cloned folder in **Visual Studio**
3. Press the **Run button** (or press **F5**) to start the application

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
│   ├── Review.cs
│   └── DiscountTier.cs              # Represents a loyalty discount tier
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
│   ├── OrderService.cs
│   └── DiscountService.cs           # Loyalty discount calculation
│
├── Factories/                       # Factory Pattern
│   └── UserFactory.cs
│
├── Strategies/                      # Strategy Pattern
│   ├── IPaymentStrategy.cs
│   └── WalletPayment.cs
│
├── States/                          # State Pattern
│   ├── IOrderState.cs
│   ├── PendingState.cs
│   ├── ProcessingState.cs
│   ├── ShippedState.cs
│   ├── DeliveredState.cs
│   └── CancelledState.cs
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
├── DataStore.cs                     # Singleton Pattern
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

## 🎨 Design Patterns

### Singleton — `DataStore.cs`
Ensures only one instance of the data store exists throughout the application. All repositories are accessed through `DataStore.Instance` so every service always reads and writes to the same data in memory.

### Factory — `UserFactory.cs`
Responsible for creating the correct user object based on the selected role. When a user registers, the factory receives the role string and returns either a `Customer` or an `Administrator` object without the calling code needing to know how each is constructed.

### Strategy — `WalletPayment.cs`
Defines a common `IPaymentStrategy` interface for processing payments. Currently implemented by `WalletPayment` which deducts the order total from the customer's wallet. A new payment method such as credit card can be added in the future by simply creating a new class that implements the same interface — no changes needed in `OrderService`.

### State — `States/`
Manages the order lifecycle by giving each status its own class. Each state defines exactly which transitions are allowed — for example `PendingState` allows moving to Processing or Cancelled but rejects Shipped or Delivered. This prevents invalid transitions entirely rather than relying on if/else checks.

```
Pending → Processing → Shipped → Delivered
    ↘           ↘
    Cancelled   Cancelled
```

---

## 🎯 OOP Concepts Demonstrated

| Concept | Implementation |
|---|---|
| **Inheritance** | `Customer` and `Administrator` inherit from `User` |
| **Polymorphism** | `DisplayInfo()` overridden in each user subclass |
| **Interfaces** | `IRepository<T>`, `IPaymentStrategy`, `IOrderState` |
| **Encapsulation** | Private fields with controlled public access |
| **Abstraction** | Services hide data access details from the console menu |

---

## 📊 Rubric Coverage

| Criteria | Implementation |
|---|---|
| System Functionality | Full customer and admin workflows |
| Object-Oriented Design | Inheritance, polymorphism, interfaces, encapsulation |
| Design Patterns | Singleton, Factory, Strategy, State |
| Console Interface | Role-based menus with input validation |
| LINQ Usage | Search, filter, sort, aggregate across all repositories |
| Exception Handling | 8 custom exceptions across all layers |
| Advanced Features | Loyalty discount system |
| Code Quality | Comments, regions, constants, single responsibility |

---

## 👨‍💻 Developer

| Field | Detail |
|---|---|
| Project Name | OnlineShoppingSystem |
| Developer | Vuyani Matshungwana |
| Type | Console Application |
| Language | C# / .NET 8 |
| Purpose | Graduate Training Project |

---

## 📄 License

This project was developed as a graduate training project. All rights reserved.