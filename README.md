# Cleaning Service Full-Stack Application

A full-stack cleaning service platform where customers can register, log in, and book cleaning services while administrators manage users and monitor activity through an admin dashboard.

This project demonstrates **ASP.NET Core full-stack development**, REST APIs, PostgreSQL integration, secure authentication, and cloud deployment.

---

# Live Demo

The application is deployed online:

https://cleaning-service-full-stack-app.onrender.com/

Hosted on **Render Cloud** with **PostgreSQL database integration**.

---

# Project Purpose

This system models a **real cleaning service workflow**.

Customers can:

- Create an account
- Log in
- Book cleaning services
- View their bookings

Administrators can:

- Manage users
- Monitor the system
- Access an admin dashboard

The system follows a **layered architecture** separating:

- UI Layer (Razor Pages)
- API Layer (REST Controllers)
- Data Layer (Entity Framework + PostgreSQL)

This design allows the system to scale toward **microservices architecture**.

---

# Tech Stack

### Backend
- ASP.NET Core
- C#
- REST API Controllers

### Frontend
- Razor Pages
- Bootstrap

### Database
- PostgreSQL
- Entity Framework Core

### Authentication & Security
- JWT Token Authentication
- Secure session handling
- Password hashing
- Role-based authorization (Admin / User)

### API Tools
- Swagger UI

### Deployment
- Render Cloud Platform

---

# Features

## Authentication System

Users can:

- Register accounts
- Log in securely
- Receive authentication tokens
- Maintain secure sessions
- Log out using the logout endpoint

Security features include:

- Password hashing
- Token authentication
- Role-based authorization

Session stores:

- UserId
- Role
- Name

Role-based redirects:

Admin → `/Admin`  
User → `/Booking`

---

# Booking System

Customers can create bookings by selecting:

- Cleaning service type
- Booking date
- Start time
- End time
- Optional notes

Each booking includes:

- Service
- Notes
- BookingDate
- StartTime
- EndTime
- Status
- CreatedAt
- CancelledAt
- UserId (Foreign Key)

Users can also view **their booking history**.

---

# Admin Dashboard

Admin users can access a management dashboard that allows them to:

- View registered users
- Delete users
- Monitor system activity

Admin navigation appears dynamically when:

```
Session["Role"] == "Admin"
```

---

# REST API Endpoints

Authentication & user management endpoints:

```
GET    /api/auth/users
POST   /api/auth/registered
POST   /api/auth/login
PUT    /api/auth/users/{id}
DELETE /api/auth/users/{id}
```

Swagger UI is enabled for API testing.

---

# Project Architecture

```
CleaningService
│
├── Pages
│   ├── Index.cshtml
│   ├── login.cshtml
│   ├── signup.cshtml
│   ├── logout.cshtml
│   ├── logout.cshtml.cs
│
│   ├── Booking
│   │   ├── Index.cshtml
│   │   └── Index.cshtml.cs
│
│   ├── Admin
│   │   ├── Index.cshtml
│   │   └── Index.cshtml.cs
│
├── Controllers
│   ├── AuthController.cs
│   └── BookingController.cs     
│
├── Models
│   ├── User.cs
│   └── Booking.cs
│
├── Data
│   └── ApplicationDbContext.cs
│
├── Program.cs
└── appsettings.json
```

---

# System Workflow

## Customer

1. Open homepage
2. Register or login
3. Redirect to booking page
4. Create cleaning booking
5. Booking stored in PostgreSQL database

---

## Admin

1. Login using admin account
2. Redirect to admin dashboard
3. View users
4. Manage user records

---

# Database

The system uses **PostgreSQL with Entity Framework Core**.

Database configuration is stored in:

```
appsettings.json
```

Admin role can be assigned manually through SQL.

Example:

```
UPDATE Users
SET Role = 'Admin'
WHERE Email = 'admin@email.com';
```

---

# Upcoming Improvements

## RabbitMQ Integration

The system will integrate **RabbitMQ** for asynchronous background processing.

Example events:

- BookingCreated
- BookingCancelled
- UserRegistered

This will allow:

- Notification services
- Background workers
- Decoupled system components

---

# AI Integration (Future)

Planned AI features include:

- Smart booking recommendations
- Predictive scheduling optimization
- AI-assisted customer support
- Intelligent service demand analysis

---

# Microservices Architecture

The system is designed to evolve into microservices such as:

- Authentication Service
- Booking Service
- Notification Service
- Admin Service
- AI Recommendation Service

Services will communicate through **REST APIs and RabbitMQ events**.

---
