# EShop

A console-based e-commerce application written in C#.

The project simulates a basic online shop where users can browse products,
search and filter products, manage their cart, place orders, and manage
their account. Administrators have additional functionality for managing
the shop.

## Technologies

- C#
- .NET
- Entity Framework Core
- PostgreSQL
- BCrypt.Net-Next
- Dependency Injection
- LINQ

## Features

### Guest

- [x] Browse products
- [x] Search products
- [x] Filter products
- [x] View product details
- [x] Add products to an in-memory cart
- [x] Remove products from cart
- [x] View cart
- [x] Register
- [x] Login

### Registered User

- [x] Everything available to guests
- [x] Persistent cart stored in the database
- [x] Merge guest cart after login
- [x] View and edit account information
- [x] Change password
- [x] Delete account
- [x] View order history
- [x] View order details
- [x] Checkout

### Administrator *(in progress)*

- [x] Login as administrator
- [x] Manage products
- [x] Add products
- [x] Edit products
- [x] Delete products
- [ ] Manage users
- [ ] Manage orders

## Authentication

User authentication is implemented using:

- Email and password
- BCrypt password hashing
- User sessions
- Role-based authorization

Passwords are never stored as plain text. Passwords are hashed using
BCrypt before being stored in the database.

The application currently supports the following roles:

```text
User
Admin
