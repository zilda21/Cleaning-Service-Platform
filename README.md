# Cleaning Service Customer Portal (Razor Pages + REST API)

A full-stack web application for a cleaning service where customers can register, log in, and create service bookings. Admin users can access an admin dashboard to view and manage users (and later bookings/logs).

---

## Purpose

This project was built to support a cleaning business workflow:

- Customers create accounts and log in
- Customers book cleaning services by selecting:
  - service type
  - booking date
  - start time / end time
  - notes (optional)
- Admin can manage users and monitor the system from a dashboard

The goal is a simple, clear architecture that separates:
- **UI (Razor Pages)** for the user experience
- **REST API Controllers** for backend operations and database access

---

## Tech Stack

- **Backend Framework:** ASP.NET Core (Razor Pages + API Controllers)
- **Database:** PostgreSQL
- **ORM:** Entity Framework Core
- **Auth (Demo / Custom):** Session-based login using a `Users` table (email/password) + role (`Admin` / `User`)
- **UI:** Razor Pages + Bootstrap styling/template assets
- **API Testing:** Swagger UI

---

## Features Completed (So Far)

### Authentication (Custom)
- ✅ Sign Up (register new user)
- ✅ Login (validate email/password)
- ✅ Session storage on login:
  - `UserId`
  - `Role`
  - `Name` (optional)
- ✅ Role-based redirect:
  - Admin → `/Admin`
  - User → `/Booking`

### User Roles
- ✅ `Role` field stored in DB (`Admin` or `User`)
- ✅ Admin role can be set directly from SQL (manual for now)

### Booking
- ✅ Booking model created with:
  - `Service`
  - `Notes`
  - `BookingDate` (DateOnly)
  - `StartTime` / `EndTime` (TimeOnly)
  - `Status`, `CreatedAt`, `CancelledAt`
  - `UserId` (FK)
- ✅ Booking UI created:
  - create booking form
  - view “My bookings”
- ✅ Booking logic currently uses direct EF Core DB access inside Razor PageModel (to avoid session issues when calling API internally)

### Admin Dashboard
- ✅ Admin UI page created (users table)
- ✅ Loads users from API endpoint
- ✅ Delete users from dashboard (calls API `DELETE`)
- ✅ Navbar shows admin links only if session Role == `Admin`

### REST API
- ✅ Endpoints implemented (current):
  - `GET /api/auth/users` → list users (safe fields)
  - `POST /api/auth/registered` → register user
  - `POST /api/auth/login` → login + session set
  - `PUT /api/auth/users/{id}` → update user fields
  - `DELETE /api/auth/users/{id}` → delete user
- ✅ Swagger enabled for easy testing

---

## Project Structure (High-Level)

- `Pages/`
  - `Index.cshtml` (Home)
  - `login.cshtml`, `signup.cshtml`
  - `Booking/Index.cshtml` (Booking UI)
  - `Admin/Index.cshtml` (Admin Dashboard)
- `Controllers/`
  - `AuthController.cs` (REST endpoints for auth + user CRUD)
- `Models/`
  - `User.cs`
  - `Booking.cs`
- `Data/`
  - `ApplicationDbContext.cs`

---

## How It Works (Flow)

### Customer
1. Opens home page
2. Clicks “Book”
3. Registers or logs in
4. After successful login → redirected to `/Booking`
5. Creates booking and sees booking list

### Admin
1. Logs in with an account having `Role = "Admin"`
2. After login → redirected to `/Admin`
3. Admin dashboard appears in navbar
4. Admin can view users and delete them

---

## Database Notes

- PostgreSQL connection configured via `appsettings.json` / `appsettings.Development.json`
- Entity Framework Core migrations are used to update schema
- Admin role is set manually for now via SQL update/insert in DB client (e.g., DBeaver)

---

## What’s Next (Planned)

- 🔜 Move booking operations fully into REST API (`BookingsController`)
- 🔜 Add admin booking management (approve/cancel/update status)
- 🔜 Add logout endpoint + UI button
- 🔜 Replace plain-text password storage with hashing OR integrate ASP.NET Identity
- 🔜 Add better validation, error handling, and user-friendly messages
- 🔜 Add pagination/search in admin dashboard

---

## Disclaimer (Current Demo State)

This version uses a simple custom login with email/password stored in the `Users` table (no hashing yet). This is suitable for learning/demo purposes. A production version should use password hashing and proper authentication (Identity or JWT).

---
